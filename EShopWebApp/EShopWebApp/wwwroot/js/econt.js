async function getCountries() {
    const url = 'https://ee.econt.com/services/Nomenclatures/NomenclaturesService.getCities.json';

    // Define any additional options you need, such as headers
    const options = {
        method: 'POST', // HTTP method
        headers: {
            // Add any headers required, such as authentication tokens
            'Content-Type': 'application/json'
        },
        // You can add a request body if needed
        body: JSON.stringify({
            // Add any parameters needed for the request
        })
    };

    try {
        // Make the fetch request
        const response = await fetch(url, options);

        // Check if the request was successful
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        // Parse the response body as JSON
        const data = await response.json();

        // Process the data returned from the server
        console.log(data);
    } catch (error) {
        // Handle any errors that occurred during the fetch request
        console.error('Error:', error);
    }
}

// Call getCountries function when button is clicked
document.getElementById("getRates").addEventListener("click", getCountries);
