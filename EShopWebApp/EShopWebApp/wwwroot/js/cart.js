// ************************************************
// Shopping Cart API
// ************************************************

var shoppingCart = (function () {
    // =============================
    // Private methods and propeties
    // =============================
    cart = [];
    var userCartLoaded = sessionStorage.getItem('userCartLoaded') === 'true';

    // Constructor
    function Item(id, name, price, count) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.count = count;
    }
    
    //Load cart items from database if user is logged in
    if (!userCartLoaded) {
        const userLoggedInCookieValue = getCookie('UserLoggedIn');
        if (userLoggedInCookieValue === 'true') {

            //set in session cart to empty
            sessionStorage.setItem('shoppingCart', JSON.stringify([]));
           
            //check if shopping cart is empty


            // User is logged in, perform actions accordingly
            console.log('User is logged in.');
            //retrieve cart items from database
            
            const options = {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                },

            };
            
            fetch('/api/cartapi/getcart', options)
                .then(response => response.json())
                .then(data => {
                    // Handle the response data
                    console.log(data);
                    for (let i = 0; i < data.length; i++) {
                        const item = data[i];
                        const id = item.id;
                        const name = item.name;
                        const price = item.price;
                        const count = item.stockQuantity;
                        shoppingCart.addItemToCart(id, name, price, count);
                    }
                    userCartLoaded = true;
                    sessionStorage.setItem('userCartLoaded', 'true'); // Set flag in sessionStorage
                    displayCart();
                })
                .catch(error => {
                    // Handle any errors that occur during the fetch request
                    console.error('Error:', error);
                });

        }

    }
   
    
    // Save cart
    function saveCart() {
        sessionStorage.setItem('shoppingCart', JSON.stringify(cart));
    }

    // Load cart
    function loadCart() {
        cart = JSON.parse(sessionStorage.getItem('shoppingCart'));
    }
    if (sessionStorage.getItem("shoppingCart") != null) {
        loadCart();
    }
    

    // =============================
    // Public methods and propeties
    // =============================
    var obj = {};

    // Add to cart
    obj.addItemToCart = function (id,name, price, count) {
        for (var item in cart) {
            if (cart[item].name === name) {
                cart[item].count++;
                saveCart();
                return;
            }
        }
        var item = new Item(id,name, price,count);
        cart.push(item);
        saveCart();
    }
    // Set count from item
    obj.setCountForItem = function (name, count) {
        for (var i in cart) {
            if (cart[i].name === name) {
                cart[i].count = count;
                break;
            }
        }
    };
    // Remove item from cart
    obj.removeItemFromCart = async function (name) {
        for (var item in cart) {
            if (cart[item].name === name) {
                cart[item].count--;
                if (cart[item].count === 0) {
                    cart.splice(item, 1);
                }
                
                break;
            }
        }
        saveCart();
        
    }

    // Remove all items from cart
    obj.removeItemFromCartAll = function (name) {
        for (var item in cart) {
            if (cart[item].name === name) {
                cart.splice(item, 1);
                break;
            }
        }
        saveCart();
    }

    // Clear cart
    obj.clearCart = function () {
        cart = [];
        saveCart();
    }

    // Count cart 
    obj.totalCount =  function () {
        var totalCount = 0;
        for (var item in cart) {
            totalCount += cart[item].count;
        }
        
        
        return totalCount;
        
    }

    // Total cart
    obj.totalCart = function () {
        var totalCart = 0;
        for (var item in cart) {
            totalCart += cart[item].price * cart[item].count;
        }
        return Number(totalCart.toFixed(2));
    }

    // List cart
    obj.listCart = function () {
        var cartCopy = [];
        for (i in cart) {
            item = cart[i];
            itemCopy = {};
            for (p in item) {
                itemCopy[p] = item[p];

            }
            itemCopy.total = Number(item.price * item.count).toFixed(2);
            cartCopy.push(itemCopy)
        }
        return cartCopy;
    }

    // cart : Array
    // Item : Object/Class
    // addItemToCart : Function
    // removeItemFromCart : Function
    // removeItemFromCartAll : Function
    // clearCart : Function
    // countCart : Function
    // totalCart : Function
    // listCart : Function
    // saveCart : Function
    // loadCart : Function
    return obj;
})();


// *****************************************
// Triggers / Events
// ***************************************** 
// Add item
$('.add-to-cart').on('click', (async function (event) {
    event.preventDefault();
    event.stopPropagation();
    var id = $(this).data('id');
    var name = $(this).data('name');
    var price = $(this).data('price');
    
    
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(id)
    };

    // Send the POST request
    await fetch('/api/cartapi/addtocart', options)
        .then(response => response)
        .then(data => {
            // Handle the response data
            
            console.log(data);
        })
        .catch(error => {
            // Handle any errors that occur during the fetch request
            console.error('Error:', error);
        });
    

    shoppingCart.addItemToCart(id, name, price, 1);
    displayCart();
    
   
   

       
}));    
//But Now Button
$('.buy-it-now').on('click', (async function (event) {
    event.preventDefault();
    event.stopPropagation();
    var id = $(this).data('id');
    var name = $(this).data('name');
    var price = $(this).data('price');


    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(id)
    };

    // Send the POST request
    await fetch('/api/cartapi/addtocart', options)
        .then(response => response)
        .then(data => {
            // Handle the response data

            console.log(data);
        })
        .catch(error => {
            // Handle any errors that occur during the fetch request
            console.error('Error:', error);
        });


    shoppingCart.addItemToCart(id, name, price, 1);
    displayCart();
    redirectToCart('/cart/index');

    function redirectToCart(url) {
        // Redirect to the specified URL
        window.location.href = url;
    }



}));    


    

// Clear items
$('.clear-cart').on('click',(async function () {
    shoppingCart.clearCart();
    displayCart();
}));


function displayCart() {
    var cartArray = shoppingCart.listCart();
    var output = "";
    
    for (var i in cartArray) {
        
        var itemCount = cartArray[i].count;
        output += "<tr>"
            + "<td>" + cartArray[i].name + "</td>"
            + "<td>(" + cartArray[i].price + ")</td>"
            + "<td><div class='input-group'><button class='minus-item input-group-addon btn btn-primary' data-name='" + cartArray[i].name + "'>-</button>"
            + "<input type='number' class='item-count form-control' data-name='" + cartArray[i].name + "' value='" + cartArray[i].count + "'>"
            + "<button class='plus-item btn btn-primary input-group-addon' data-name='" + cartArray[i].name + "'>+</button></div></td>"
            + "<td><button class='delete-item btn btn-danger' data-name='" + cartArray[i].name + "'>X</button></td>"
            + " = "
            + "<td>" + cartArray[i].total + "</td>"
            + "</tr>";
        $('.cart-item-count-' + cartArray[i].id + '').html(cartArray[i].count);
    }
    $('.show-cart').html(output);
    $('.total-cart').html(shoppingCart.totalCart());
    $('.total-count').html(shoppingCart.totalCount());
}

// Delete item button

$('.show-cart').on("click", ".delete-item", function (event) {
    var name = $(this).data('name')
    for (var item in cart) {
        if (cart[item].name === name) {
            const options = {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(cart[item].id)
            };
            fetch('/api/cartapi/removecartitem', options)
                .then(response => response.json())
                .then(data => {
                    // Handle the response data
                    console.log(cart[item].id);
                })
                .catch(error => {
                    // Handle any errors that occur during the fetch request
                    console.error('Error:', error);
                });
         }
    }
    // Hide the element if the count is 0
    for (const item in cart) {
        if (cart[item].count === 1 && cart[item].name === name) {
            // Construct the ID of the element to hide
            const elementId = 'item-' + cart[item].id;

            // Hide the element
            const element = document.querySelector('.' + elementId);
            if (element) {
                element.classList.add("visually-hidden");
            }
        }
    }
    shoppingCart.removeItemFromCartAll(name);
    displayCart();
})


// -1
$('.show-cart').on('click', '.minus-item', async function (event) {
    event.preventDefault();
    event.stopPropagation();
    var name = $(this).data('name')
    var id = $(this).data('id')
    for (var item in cart) {

        if (cart[item].name === name) {
            id = cart[item].id;
            const options = {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(id)
            };
            await fetch('/api/cartapi/removefromcart', options)
                .then(response => response.json())
                .then(data => {
                    // Handle the response data
                    console.log(id);
                })
                .catch(error => {
                    // Handle any errors that occur during the fetch request
                    console.error('Error:', error);
                });

        }
    }
    // Hide the element if the count is 0
    for (const item in cart) {
        if (cart[item].count === 1 && cart[item].name === name) {
            // Construct the ID of the element to hide
            const elementId = 'item-' + cart[item].id;

            // Hide the element
            const element = document.querySelector('.' + elementId);
            if (element) {
                element.classList.add("visually-hidden");
            }
        }
    }
    
    shoppingCart.removeItemFromCart(name);
    displayCart();
})
// +1
$('.show-cart').on('click', '.plus-item', async function (event) {
    event.preventDefault();
    event.stopPropagation();
    var id = $(this).data('id')
    var name = $(this).data('name')
    var price = $(this).data('price')
    var count = $(this).data('count')
    for (var item in cart) {


        if (cart[item].name === name) {
            const options = {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(cart[item].id)
            };
            await fetch('/api/cartapi/addtocart', options)
                .then(response => response.json())
                .then(data => {
                    // Handle the response data
                    console.log(cart[item].id);
                })
                .catch(error => {
                    // Handle any errors that occur during the fetch request
                    console.error('Error:', error);
                });

        }
    }

    shoppingCart.addItemToCart(id,name,price,count);
    displayCart();
})

// Item count input
$('.show-cart').on('change', '.item-count', function (event) {
    var name = $(this).data('name');
    var count = Number($(this).val());
    shoppingCart.setCountForItem(name, count);
    displayCart();
});
//Remove item from cart on order page
$('.cart-item').on('click', '.minus-item', async function (event) {
    event.preventDefault();
    event.stopPropagation();
    var name = $(this).data('name')
    var id = $(this).data('id')
            const options = {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(id)
            };
            await fetch('/api/cartapi/removefromcart', options)
                .then(response => response)
                .then(data => {
                    // Handle the response data
                    console.log(id);
                })
                .catch(error => {
                    // Handle any errors that occur during the fetch request
                    console.error('Error:', error);
                });
                // Hide the element if the count is 0
   
    
    shoppingCart.removeItemFromCart(name);
    displayCart();
})
$('.cart-item').on('click', '.plus-item', async function (event) {
    event.preventDefault();
    event.stopPropagation();
    var name = $(this).data('name')
    var id = $(this).data('id')
    var price = $(this).data('price')
    var count = $(this).data('count')
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(id)
    };
    await fetch('/api/cartapi/addtocart', options)
        .then(response => response)
        .then(data => {
            // Handle the response data
            console.log({ id: id });
        })
        .catch(error => {
            // Handle any errors that occur during the fetch request
            console.error('Error:', error);
        });


    shoppingCart.addItemToCart(id, name, price, count);
    displayCart();
})
//Load cart items ON LOGIN //TO DO !!!!
$('.logout-flag').on('click', (async function (event) {

    
    
    sessionStorage.setItem('userCartLoaded', 'false');
   
}));
$('.load-cart-items-login').on('click', (async function (event) {
    event.preventDefault();
    event.stopPropagation();
    const options = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        
    };

    // Send the POST request
    await fetch('/api/cartapi/getcart', options)
        .then(response => response)
        .then(data => {
            // Handle the response data

            console.log(data);
        })
        .catch(error => {
            // Handle any errors that occur during the fetch request
            console.error('Error:', error);
        });


    shoppingCart.addItemToCart(id, name, price, 1);
    displayCart();



}));    
function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].trim();
        // Check if this cookie name is the one we're looking for
        if (cookie.startsWith(name + '=')) {
            // Return the cookie value
            return cookie.substring(name.length + 1);
        }
    }
    // Return null if cookie not found
    return null;
}





displayCart();
