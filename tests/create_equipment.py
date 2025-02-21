import asyncio
from aiohttp import ClientSession, ClientTimeout

CREATE_EQUIPMENT_ENDPOINT = "/api/Equipment/create_equipment"
BASE_URL = "http://localhost:8000"
AUTH_ENDPOINT = "/api/Employee/authorization"

# Словарь для хранения спецификаций комплектующих
COMPONENTS = {
    "cpu_gaming": {"name": "Intel i7-12700K", "specifications": "Gaming CPU", "price": 400},
    "gpu_gaming": {"name": "NVIDIA RTX 4080", "specifications": "Gaming GPU", "price": 900},
    "ram_gaming": {"name": "32GB DDR5", "specifications": "Gaming RAM", "price": 200},
    "storage_gaming": {"name": "2TB NVMe SSD", "specifications": "Gaming Storage", "price": 200},
    "monitor_gaming": {"name": "32-inch 144Hz Monitor", "specifications": "Gaming Monitor", "price": 300},
    "keyboard_gaming": {"name": "Mechanical Gaming Keyboard", "specifications": "Gaming Keyboard", "price": 100},
    "mouse_gaming": {"name": "Gaming Mouse", "specifications": "Gaming Mouse", "price": 50},

    "cpu_regular": {"name": "Intel i5-12400", "specifications": "Regular CPU", "price": 200},
    "gpu_regular": {"name": "NVIDIA RTX 3060", "specifications": "Regular GPU", "price": 400},
    "ram_regular": {"name": "16GB DDR4", "specifications": "Regular RAM", "price": 100},
    "storage_regular": {"name": "1TB SSD", "specifications": "Regular Storage", "price": 100},
    "monitor_regular": {"name": "24-inch 60Hz Monitor", "specifications": "Regular Monitor", "price": 150},
    "keyboard_regular": {"name": "Regular Keyboard", "specifications": "Regular Keyboard", "price": 50},
    "mouse_regular": {"name": "Regular Mouse", "specifications": "Regular Mouse", "price": 30}
}

async def get_token(session: ClientSession):
    async with session.post(BASE_URL + AUTH_ENDPOINT,
                             json={
        "login": "Owner",
        "password": "12345678"
    }) as response:
        result = await response.json()
        token = result["token"]
        return token

async def fetch(session: ClientSession, token: str, eq_type: str, name: str,
                specifications: str, price: int, id_club: int):
    try:
        async with session.post(
            BASE_URL + CREATE_EQUIPMENT_ENDPOINT,
            headers={"Authorization": f"Bearer {token}"},
                json={
                    "type": eq_type,
                    "name": name,
                    "specification": specifications,
                    "purchasePrice": price,
                    "idClub": id_club
                },
            timeout=ClientTimeout(total=30)  # Увеличиваем таймаут до 30 секунд
        ) as response:
            result = await response.json()
            print(f"Status: {response.status}, Response: {result}")
    except Exception as e:
        print(f"Error: {e}")

async def main():
    async with ClientSession() as session:
        token = await get_token(session)
        if not token:
            print("Unauthorized")
            return

        semaphore = asyncio.Semaphore(10)  # Ограничиваем количество одновременных запросов до 10

        tasks = []

        for club_id in range(1, 21):  # 20 компьютерных клубов
            for pc_num in range(1, 16):  # 15 мест в каждом клубе
                is_gaming = pc_num <= 10  # Первые 10 ПК - игровых, остальные - обычные

                # Определяем тип оборудования
                prefix = "gaming" if is_gaming else "regular"

                # Создаем задачи для каждого компонента
                components = [
                    ("cpu", COMPONENTS[f"cpu_{prefix}"]),
                    ("gpu", COMPONENTS[f"gpu_{prefix}"]),
                    ("ram", COMPONENTS[f"ram_{prefix}"]),
                    ("storage", COMPONENTS[f"storage_{prefix}"]),
                    ("monitor", COMPONENTS[f"monitor_{prefix}"]),
                    ("keyboard", COMPONENTS[f"keyboard_{prefix}"]),
                    ("mouse", COMPONENTS[f"mouse_{prefix}"])
                ]

                for component_name, component_data in components:
                    task = asyncio.ensure_future(
                        bounded_fetch(
                            semaphore,
                            session,
                            token,
                            eq_type=prefix,
                            name=f"{component_data['name']} (PC {pc_num})",
                            specifications=component_data["specifications"],
                            price=component_data["price"],
                            id_club=club_id
                        )
                    )
                    tasks.append(task)

        await asyncio.gather(*tasks)

async def bounded_fetch(semaphore: asyncio.Semaphore, *args, **kwargs):
    """Ограничивает количество одновременных запросов."""
    async with semaphore:
        await fetch(*args, **kwargs)

if __name__ == "__main__":
    asyncio.run(main())