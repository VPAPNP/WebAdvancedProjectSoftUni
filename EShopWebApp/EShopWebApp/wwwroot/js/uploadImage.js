jQuery(document).ready(function () {
   
   
    
    ImgUpload();

    
});


function fetchImageDataByGuid(imageGuid) {
    // Make an HTTP GET request to the API endpoint
    return fetch(`/api/PhotoApiController/getproductphotos?id=${imageGuid}`)
        .then(response => {
            // Check if the response is successful
            if (!response.ok) {
                throw new Error('Failed to fetch image data');
            }
            // Parse the response JSON
            return response.json();
        })
        .then(data => {
            // Assuming the API returns image data and IDs as an object with 'data' and 'id' properties
            // You may need to adjust this part based on the actual response format
            return { id: data.id, imageData: data.data };
        })
        .catch(error => {
            console.error('Error fetching image data:', error);
            throw error; // Rethrow the error for handling in the calling code
        });
}
function ImgUpload() {
    var imgWrap = "";
    var imgArray = [];

    $('.upload__inputfile').each(function () {
        $(this).on('change', function (e) {
            imgWrap = $(this).closest('.upload__box').find('.upload__img-wrap');
            var maxLength = $(this).attr('data-max_length');

            var files = e.target.files;
            var filesArr = Array.prototype.slice.call(files);
            var iterator = 0;
            filesArr.forEach(function (f, index) {

                if (!f.type.match('image.*')) {
                    return;
                }

                if (imgArray.length > maxLength) {
                    return false
                } else {
                    var len = 0;
                    for (var i = 0; i < imgArray.length; i++) {
                        if (imgArray[i] !== undefined) {
                            len++;
                        }
                    }
                    if (len > maxLength) {
                        return false;
                    } else {
                        imgArray.push(f);

                        var reader = new FileReader();
                        reader.onload = function (e) {
                            var html = "<div class='upload__img-box'><div style='background-image: url(" + e.target.result + ")' data-number='" + $(".upload__img-close").length + "' data-file='" + f.name + "' class='img-bg'><div class='upload__img-close'></div></div></div>";
                            imgWrap.append(html);
                            iterator++;
                        }
                        reader.readAsDataURL(f);
                    }
                }
            });
        });
        
        

        
        
    });
   

    $('body').on('click', ".upload__img-close", function (e) {
       var id = $(this).data("id");
        var file = $(this).parent().data("file");
        for (var i = 0; i < imgArray.length; i++) {
            if (imgArray[i].name === file) {
                imgArray.splice(i, 1);
                break;
            }
        }
        $(this).parent().parent().remove();
    });
    $('body').on('click', ".edit", function (e)
    {
        var id = $(this).data("id");
        const url = '/api/photoapi/deletephoto';

        const options = {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(id)
        };
        fetch('/api/photoapi/deletephoto', options)
            .then(response => response.json())
            .then(data => {
                // Handle the response data
                console.log(id);
            })
            .catch(error => {
                // Handle any errors that occur during the fetch request
                console.error('Error:', error);
            });
       
    });
}