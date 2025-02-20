import asyncio
import logging
from faker import Faker
from aiohttp import ClientSession
from typing import List, Dict, Tuple

# Настройка логирования
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s',
    handlers=[logging.FileHandler('clients_test.log'), logging.StreamHandler()]
)
logger = logging.getLogger(__name__)

BASE_URL = "http://localhost:8000"
CREATE_CLIENT_ENDPOINT = "/api/Client/create_client"
MAX_CLIENTS = 200
MAX_CONCURRENT_REQUESTS = 50  # Ограничение параллельных запросов

fake = Faker()

def generate_valid_client_data() -> Dict:
    """Генерация валидных данных клиента"""
    return {
        "firstName": fake.first_name(),
        "middleName": fake.first_name(),
        "lastName": fake.last_name(),
        "email": fake.email(),
        "login": fake.user_name()[:3] + fake.user_name(),  # Гарантированно >= 3 символов
        "password": fake.password(length=12)               # Гарантированно >= 8 символов
    }

def generate_invalid_data_cases() -> List[Tuple[Dict, str]]:
    """Генерация тестовых случаев с невалидными данными"""
    return [
        (
            {**generate_valid_client_data(), "login": "ab"},
            "Слишком короткий логин"
        ),
        (
            {**generate_valid_client_data(), "password": "short"},
            "Слишком короткий пароль"
        )
    ]

async def create_client(session: ClientSession, client_data: Dict) -> Tuple[int, Dict]:
    """Отправка запроса на создание клиента"""
    try:
        async with session.post(
                f"{BASE_URL}{CREATE_CLIENT_ENDPOINT}",
                json=client_data
        ) as response:
            response_data = await response.json() if response.content_length else {}
            return response.status, response_data
    except Exception as e:
        logger.error(f"Ошибка при создании клиента: {str(e)}")
        return 500, {}

async def worker(session: ClientSession, queue: asyncio.Queue):
    """Асинхронный воркер для обработки задач из очереди"""
    while not queue.empty():
        client_data, test_case = await queue.get()
        status, response = await create_client(session, client_data)

        log_message = (
            f"Статус: {status} | "
            f"Логин: {client_data['login']} | "
            f"Тест: {test_case or 'Валидные данные'}"
        )

        if status == 200 or status == 201:
            logger.info(log_message)
        else:
            logger.error(f"{log_message} | Ошибка: {response.get('errors', '')}")

async def main():
    """Основная функция выполнения тестов"""
    # Генерация тестовых данных
    tasks = []
    for _ in range(MAX_CLIENTS):
        tasks.append((generate_valid_client_data(), ""))

    # Добавление невалидных случаев
    for data, desc in generate_invalid_data_cases():
        tasks.append((data, desc))

    # Создание очереди задач
    queue = asyncio.Queue()
    for task in tasks:
        await queue.put(task)

    # Запуск асинхронных воркеров
    async with ClientSession() as session:
        workers = [
            worker(session, queue)
            for _ in range(MAX_CONCURRENT_REQUESTS)
        ]
        await asyncio.gather(*workers)

if __name__ == "__main__":
    logger.info("Запуск тестов создания клиентов...")
    asyncio.run(main())
    logger.info("Тестирование завершено. Проверьте лог для деталей.")