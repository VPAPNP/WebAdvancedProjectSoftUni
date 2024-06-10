async function getWeight() {
    try {
        let response = await fetch('/api/cartapi/getweight');
        let data = await response.json();
        console.log(data); // Handle the response data
        return data;
    } catch (error) {
        console.error('Error:', error); // Handle any errors that occur during the fetch request
        return null;
    }
}
document.addEventListener("DOMContentLoaded", async function () {
    var shippingPrice = 0;

    
    function updateTotalPrice() {
        const subtotalElement = Array.from(document.querySelectorAll('.total-data td')).find(td => td.textContent.includes("Subtotal"));
        const subtotal = parseFloat(subtotalElement.nextElementSibling.textContent.replace('$', ''));
        shippingPrice = parseFloat(document.getElementById('shippingPrice').textContent);
        const total = subtotal + shippingPrice;
        $('#totalPrice').html(total.toFixed(2));
        





       // const totalElement = Array.from(document.querySelectorAll('.total-data td')).find(td => td.textContent.includes("Total"));
        //totalElement.nextElementSibling.textContent = `$${total.toFixed(2)}`;
    }
    async function getPrice() {
        const paymentMethod = document.getElementById("paymentMethod").value;
        const shippingMethod = document.getElementById("shippingMethod").value;

        var weight = await getWeight();

        const subtotalElement = Array.from(document.querySelectorAll('.total-data td')).find(td => td.textContent.includes("Subtotal"));
        const subtotal = parseFloat(subtotalElement.nextElementSibling.textContent.replace('$', ''));

        if (paymentMethod === "pickup") {
            // For pickup, no shipping price and only show name and phone fields
            document.getElementById('shippingPrice').textContent = '0';
            updateTotalPrice();
            hideAddressFields();
            return;
        } else {
            showAddressFields();
        }
        if (shippingMethod === "вземи от място") {
            // If this block should have logic, add it here.
        }

        const url = "https://ee.econt.com/services/Shipments/LabelService.createLabel.json";

        const payload = {
            "label": {
                "senderClient": {
                    "name": "Иван Иванов",
                    "phones": ["0888888888"]
                },
                "senderAddress": {
                    "city": {
                        "country": {
                            "code3": "BGR"
                        },
                        "name": "Русе",
                        "postCode": "7012"
                    },
                    "street": "Алея Младост",
                    "num": "7"
                },
                "receiverClient": {
                    "name": document.getElementById("receiverName").value,
                    "phones": [document.getElementById("receiverPhone").value]
                },
                "receiverAddress": {
                    "city": {
                        "country": {
                            "code3": "BGR"
                        },
                        "name": document.getElementById("receiverCity").value,
                        "postCode": document.getElementById("receiverPostCode").value
                    },
                    "street": document.getElementById("receiverStreet").value,
                    "num": document.getElementById("receiverNum").value,
                    "other": document.getElementById("receiverOther").value
                },
                "packCount": 1,
                "shipmentType": "pallet",
                "weight": weight,
                "shipmentDimensionsL": 120,
                "shipmentDimensionsW": 80,
                "shipmentDimensionsH": 180,
                "shipmentDescription": "обувки",
                "receiverOfficeCode": "3407",
                "receiverDeliveryType": shippingMethod === "pickup" ? "office" : "door",
                "services": {
                    "cdAmount": paymentMethod === "delivery" ? subtotal : 0,
                    "cdType": paymentMethod === "delivery" ? "get" : "",
                    "cdCurrency": paymentMethod === "delivery" ? "bgn" : ""
                }
            },
            "mode": "calculate"
        };

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(payload)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const data = await response.json();
            shippingPrice = data.label.totalPrice;
            //document.getElementById('shippingPrice').textContent = data.label.totalPrice;
            $('#shippingPrice').html(shippingPrice);
            updateTotalPrice();
        } catch (error) {
            console.error("Error:", error);
        }
    }

    // Function to update the total price
   

    // Functions to show/hide address fields
    function hideAddressFields() {
        document.getElementById('receiverCityGroup').style.display = 'none';
        document.getElementById('receiverPostCodeGroup').style.display = 'none';
        document.getElementById('receiverStreetGroup').style.display = 'none';
        document.getElementById('receiverNumGroup').style.display = 'none';
        document.getElementById('receiverOtherGroup').style.display = 'none';
    }

    function showAddressFields() {
        document.getElementById('receiverCityGroup').style.display = 'block';
        document.getElementById('receiverPostCodeGroup').style.display = 'block';
        document.getElementById('receiverStreetGroup').style.display = 'block';
        document.getElementById('receiverNumGroup').style.display = 'block';
        document.getElementById('receiverOtherGroup').style.display = 'block';
    }

    // Add event listeners to the relevant fields to update the price dynamically
    document.getElementById('paymentMethod').addEventListener('change', getPrice);
    document.getElementById('shippingMethod').addEventListener('change', getPrice);
    document.getElementById('receiverCity').addEventListener('change', getPrice);
    document.getElementById('receiverPostCode').addEventListener('change', getPrice);
    document.getElementById('receiverStreet').addEventListener('change', getPrice);
    document.getElementById('receiverNum').addEventListener('change', getPrice);
    document.getElementById('receiverOther').addEventListener('change', getPrice);
    $('.cart-item').on('change', '.item-count', getPrice);


    // Initial call to set up the form based on default values
    getPrice();
});



