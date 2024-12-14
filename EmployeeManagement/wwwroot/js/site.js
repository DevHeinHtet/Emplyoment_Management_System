function previewImage(event) {
    const input = event.target;
    const preview = document.getElementById('imagePreview');

    if (input.files && input.files[0]) {
        const file = input.files[0];

        // Ensure the file is an image
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = function (e) {
                preview.src = e.target.result; // Set the preview src to the image data
            };
            reader.readAsDataURL(file); // Read the file and trigger the onload event
        } else {
            alert("Please select a valid image file."); // Error handling for non-image files
            input.value = ""; // Reset the input
        }
    } else {
        preview.src = "https://via.placeholder.com/150"; // Reset to placeholder if no file selected
    }
}

// Preview NRC Front Image
function previewNrcFront(event) {
    const input = event.target;
    const preview = document.getElementById('nrcFrontPreview');

    if (input.files && input.files[0]) {
        const file = input.files[0];

        // Ensure the file is an image
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = function (e) {
                preview.src = e.target.result;
            };
            reader.readAsDataURL(file);
        } else {
            alert("Please select a valid image file.");
            input.value = "";
        }
    } else {
        preview.src = "https://blackbull.com/wp-content/uploads/2023/09/id-placeholder-fron.jpg";
    }
}

// Preview NRC Back Image
function previewNrcBack(event) {
    const input = event.target;
    const preview = document.getElementById('nrcBackPreview');

    if (input.files && input.files[0]) {
        const file = input.files[0];

        // Ensure the file is an image
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = function (e) {
                preview.src = e.target.result;
            };
            reader.readAsDataURL(file);
        } else {
            alert("Please select a valid image file.");
            input.value = "";
        }
    } else {
        preview.src = "https://blackbull.com/wp-content/uploads/2023/09/id-placeholder-back.jpg";
    }
}