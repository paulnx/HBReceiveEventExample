# Hirebrdge example of how to receive web hooks/send events
Hirebridge example of how to receive send events/web hooks in an Azure Function

clone this project in Visual studios.  You will need the Azure function add on.

Use postman to post to http://localhost:7071/api/ReceiveEvent    Use the  contents of InputExample.json  
to simulate a Hirebridge transmitting a webhook event.  Or import into postman collection HirebridgeSendEventExample.postman_collection.json


Data is transformed and transmitted to https://ptsv2.com/t/random411  as an example.