import asyncio 
import faker 
from faker import Faker
from aiohttp import ClientSession
import logging

logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s',
    handlers=[logging.FileHandler('clients_test.log'), logging.StreamHandler()]
)
logger = logging.getLogger(__name__)

MAX_CLUBS = 20
BASE_URL = "http://localhost:8000"
CREATE_WORKING_SPACE_ENDPOINT = "/api/WorkingSpace/create_working_space"
AUTH_ENDPOINT = "/api/Employee/authorization"

async def get_token(session: ClientSession):
    async with session.post(BASE_URL + AUTH_ENDPOINT,
                             json={
        "login": "Owner",
        "password": "12345678"
    }) as response:
        result = await response.json()
        token = result["token"]
        return token
    
async def create_space(session: ClientSession, token: str, id_club: int, name: str, status: str, id_tariff: int):
    async with session.post(BASE_URL + CREATE_WORKING_SPACE_ENDPOINT,
                            headers={
                                "Authorization": f"Bearer {token}"
                            },
                            json={
                                "idClub": id_club,
                                "name": name,
                                "status": status,
                                "idTariff": id_tariff
                            }
                            ) as response:
        result = await response.json()
        print(result)
    
async def main():
    async with ClientSession() as session:
        token = await get_token(session)
        if not token:
            print("Unauthorized")
            return
        tasks = []
        for i in range(1, 21): # Клубы
            for j in range(1, 16): # Места
                tasks.append(create_space(session, token, i, f"space{j}", "string", 1))
        await asyncio.gather(*tasks)


asyncio.run(main())



                