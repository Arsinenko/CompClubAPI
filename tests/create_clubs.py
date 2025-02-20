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
CREATE_CLUB_ENDPOINT = "/api/Clubs/create_club"
AUTH_ENDPOINT = "/api/Employee/authorization"

async def get_token(session: ClientSession):
    async with session.post(BASE_URL + AUTH_ENDPOINT,
                             json={
        "login": "Owner",
        "password": "12345678"
    }) as response:
        print(await response.json())
        return await response.json().get("token")
    
async def create_club(session: ClientSession, token: str, club_number: int):
    async with session.post(BASE_URL + CREATE_CLUB_ENDPOINT,
                            headers={
                                "Authorization": f"Bearer {token}"
                            },
                            json={
                                "address": f"Address {club_number}",
                                "name": f"Club {club_number}",
                                "phone": f"+123456789{club_number}",
                                "workingHours": "9:00-22:00",
                                "finances": 0
                            }) as response:
        logger.log(f"{response.status}: {response.content}")

async def main():
    async with ClientSession() as session:
        token = await get_token(session)
        if not token:
            print("Unauthorized")
            return
        tasks = [create_club(session, token, i) for i in range(2, 21)]
        await asyncio.gather(*tasks)


asyncio.run(main())



                