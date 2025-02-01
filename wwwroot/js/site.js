// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.getElementById('searchButton').addEventListener('click', function () {
    const searchInput = document.getElementById('searchInput').value;
    if (searchInput) {
        alert(`Searching for: ${searchInput}`);
    }
});

document.addEventListener("DOMContentLoaded", () => {
    let currentIndex = 0;
    const slides = document.querySelectorAll('.carousel-item');
    const totalSlides = slides.length;

    if (totalSlides === 0) {
        console.error("Carousel items not found");
        return;
    }

    // İlk slaytı göster
    function showSlide(index) {
        slides.forEach((slide, i) => {
            slide.style.display = i === index ? 'block' : 'none';
        });
    }

    // Sonraki slayta geçiş
    function nextSlide() {
        currentIndex = (currentIndex + 1) % totalSlides;
        showSlide(currentIndex);
    }

    // Önceki slayta geçiş
    function prevSlide() {
        currentIndex = (currentIndex - 1 + totalSlides) % totalSlides;
        showSlide(currentIndex);
    }

    // Slayt kontrol düğmelerini bağla
    const nextButton = document.querySelector('.carousel-control-next');
    const prevButton = document.querySelector('.carousel-control-prev');

    if (nextButton) nextButton.addEventListener('click', nextSlide);
    if (prevButton) prevButton.addEventListener('click', prevSlide);

    // İlk slaytı göster
    showSlide(currentIndex);
});

document.addEventListener("DOMContentLoaded", () => {
    const seeDetailsLink = document.getElementById("seeDetailsLink");
    const popup = document.getElementById("treePopup");
    const closePopup = document.getElementById("closePopup");

    // Popup'ı aç
    seeDetailsLink.addEventListener("click", (event) => {
        event.preventDefault(); // Sayfa yenilemeyi önle
        popup.style.display = "block";
    });

    // Popup'ı kapat
    closePopup.addEventListener("click", () => {
        popup.style.display = "none";
    });

    // Popup dışında bir yere tıklanınca kapat
    window.addEventListener("click", (event) => {
        if (event.target === popup) {
            popup.style.display = "none";
        }
    });
});





