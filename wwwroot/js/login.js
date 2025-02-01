document.addEventListener("DOMContentLoaded", () => {
    const slides = document.querySelectorAll(".slider-image");
    let currentIndex = 0;

    const showSlide = (index) => {
        slides.forEach((slide, i) => {
            slide.classList.remove("active");
            if (i === index) slide.classList.add("active");
        });
    };

    setInterval(() => {
        currentIndex = (currentIndex + 1) % slides.length;
        showSlide(currentIndex);
    }, 3000); // 3 saniyede bir geçiş

    // Şifre Görünürlüğü
    const togglePassword = document.querySelector(".toggle-password");
    const passwordInput = document.getElementById("password");

    togglePassword.addEventListener("click", () => {
        const type = passwordInput.getAttribute("type") === "password" ? "text" : "password";
        passwordInput.setAttribute("type", type);
        togglePassword.classList.toggle("fa-eye-slash");
    });
});
