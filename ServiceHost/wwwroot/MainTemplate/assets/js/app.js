// When the document is ready
$(function () {
    // Enable Bootstrap tooltips for elements with 'data-bs-toggle="tooltip"'
    jQuery('[data-bs-toggle="tooltip"]').tooltip();

    // Enable tooltips for modal-triggering elements that have a 'title' attribute
    jQuery('[data-bs-toggle="modal"][title]').tooltip();
});

// Scroll to the top of the page when the function is called
function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE, and Opera
}

/**
 * Copy the current page URL to the clipboard
 */
const btnCopy = document.querySelector(".btnCopy");
if (btnCopy) {
    btnCopy.addEventListener("click", () => {
        navigator.clipboard.writeText(window.location.href); // Copy URL to clipboard
        alert("لینک کپی شد"); // Show alert that the link has been copied
    });
}

/**
 * Handle story video playback in a modal
 */
function stories() {
    const showStoryBtn = document.querySelectorAll('.showStoryBtn'); // Select all story buttons
    const videoStory = document.getElementById('videoStory'); // Select the story video element
    const storyModal = document.getElementById('storyModal'); // Select the story modal

    // Add event listeners to all story buttons
    showStoryBtn.forEach(showStoryBtnItem => showStoryBtnItem.addEventListener('click', function () {
        try {
            videoStory.src = showStoryBtnItem.getAttribute('data-story'); // Set video source
            videoStory.play(); // Play the video
        } catch (e) {
            console.log(e); // Log any errors
        }
    }));

    // Clear video source when the modal is closed
    storyModal.addEventListener('hidden.bs.modal', () => {
        videoStory.src = "";
    });
}

// Video play button functionality
document.addEventListener("DOMContentLoaded", () => {
    const video = document.getElementById("aboutVideo"); // Select the video element
    const playButton = document.querySelector(".play-btn"); // Select the play button
    const playIcon = document.getElementById("play-icon"); // Select the play icon inside the button

    // Ensure all required elements exist before proceeding
    if (!video || !playButton || !playIcon) {
        return;
    }

    // Toggle play/pause when the play button is clicked
    playButton.addEventListener("click", () => {
        if (video.paused) {
            video.play(); // Play the video
            playButton.style.display = "none"; // Hide play button when playing
        } else {
            video.pause(); // Pause the video
        }
    });

    // Show play button and update icon when the video is paused
    video.addEventListener("pause", () => {
        playButton.style.display = "flex"; // Show play button again
        playIcon.className = "bi bi-play-fill"; // Switch to play icon
    });

    // Update icon when the video starts playing
    video.addEventListener("play", () => {
        playIcon.className = "bi bi-pause-fill"; // Switch to pause icon
    });
});

/**
 * Function to select a color by adding a 'selected' class to the clicked element
 */
function selectColor(element) {
    // Remove 'selected' class from all color boxes
    document.querySelectorAll('.color-box').forEach(box => {
        box.classList.remove('selected');
    });

    // Add 'selected' class to the clicked element
    element.classList.add('selected');
}
