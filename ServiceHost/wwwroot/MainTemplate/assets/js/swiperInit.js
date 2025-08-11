//========= Start Free Mode ==============//
let swiperFreeMode = new Swiper(".my-unique-free-mode", {
    slidesPerView: "auto",
    spaceBetween: 10,
    freeMode: true,
    navigation: {
        nextEl: ".swiper-button-next-unique",
        prevEl: ".swiper-button-prev-unique",
    },
});
//========= End Free Mode ==============//

//=========== Start Product Gallery ===============//
let proSwiper = new Swiper(".product-gallery-thumb", {
    spaceBetween: 10,
    slidesPerView: 4,
    freeMode: true,
    watchSlidesProgress: true,
    breakpoints: {
        320: { // Responsive settings for smaller screens
            slidesPerView: 3,
            spaceBetween: 10
        },
    },
});

let proThumbswiper = new Swiper(".product-gallery", {
    spaceBetween: 10,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    zoom: { // Enable zoom functionality
        maxRatio: 3,
        minRatio: 1
    },
    thumbs: {
        swiper: proSwiper, // Connect with thumbnail swiper
    },
});
//=========== End Product Gallery ===============//

//========= Start Product Box ==============//
let swiperProSlider = new Swiper(".pro-slider", {
    slidesPerView: 5,
    spaceBetween: 10,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        100: { slidesPerView: 1 },
        576: { slidesPerView: 2 },
        768: { slidesPerView: 3 },
        1024: { slidesPerView: 4 },
        1400: { slidesPerView: 5 }
    },
});
//========= End Product Box ==============//

//========= Start Product Box with Cover ==============//
let swiperProSliderWithCover = new Swiper(".pro-slider-with-cover", {
    slidesPerView: 3,
    spaceBetween: 10,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        100: { slidesPerView: 1 },
        576: { slidesPerView: 1 },
        768: { slidesPerView: 2 },
        1024: { slidesPerView: 3 },
        1400: { slidesPerView: 3 }
    },
});
//========= End Product Box with Cover ==============//

//========= Start Offer Section ==============//
var swiperOfferItemLink = new Swiper("#offerItemLink", {
    spaceBetween: 10,
    slidesPerView: 4,
    freeMode: true,
    watchSlidesProgress: true,
    allowTouchMove: false,
});

var swiperOfferItem = new Swiper("#offerItem", {
    effect: "fade",
    speed: 1000,
    loop: true,
    autoplay: {
        delay: 5000,
        disableOnInteraction: false,
    },
    spaceBetween: 10,
    thumbs: {
        swiper: swiperOfferItemLink,
    },
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
});
//========= End Offer Section ==============//

//========= Start Blog Slider ==============//
var blogSliderSw = new Swiper(".blog-slider-sw", {
    slidesPerView: 3,
    spaceBetween: 10,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        100: { slidesPerView: 1 },
        768: { slidesPerView: 2 },
        1024: { slidesPerView: 3 },
    }
});
//========= End Blog Slider ==============//

//=========== Start Small Slider ===========//
var smallSlider = new Swiper(".swiper-small-slider", {
    spaceBetween: 10,
    slidesPerView: 1,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    loop: true,
    speed: 2500,
    autoplay: {
        delay: 3500,
        disableOnInteraction: false,
    },
});
//=========== End Small Slider ===========//

//=========== Start Category Slider ===========//
var catSlider = new Swiper(".cat-slider", {
    slidesPerView: 5,
    spaceBetween: 10,
    loop: true,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    autoplay: {
        delay: 3000,
        disableOnInteraction: false,
    },
    breakpoints: {
        100: { slidesPerView: 1 },
        575: { slidesPerView: 2 },
        1024: { slidesPerView: 4 },
    },
});
//=========== End Category Slider ===========//

//=========== Start Amazing Slider ===========//
var amazing = new Swiper(".amazing", {
    slidesPerView: "auto",
    spaceBetween: 10,
    freeMode: true,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
});
//=========== End Amazing Slider ===========//

//========= Start Product Slider =========//
var proSlider = new Swiper("#product-slider", {
    slidesPerView: 5,
    spaceBetween: 10,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        100: { slidesPerView: 1, spaceBetween: 20 },
        576: { slidesPerView: 2, spaceBetween: 20 },
        768: { slidesPerView: 2, spaceBetween: 20 },
        1024: { slidesPerView: 3, spaceBetween: 20 },
        1200: { slidesPerView: 4, spaceBetween: 20 },
    },
});
//========= End Product Slider =========//

//========= Start Home Slider =========//
var swiperMainSlider = new Swiper("#homeSlider", {
    spaceBetween: 30,
    centeredSlides: true,
    loop: true,
    autoplay: {
        delay: 5500,
        disableOnInteraction: false,
    },
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
});
//========= End Home Slider =========//

//========= Start Product Slider with Swiper =========//
var swiper = new Swiper(".product-slider-swiper", {
    slidesPerView: 5,
    spaceBetween: 10,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        100: { slidesPerView: 1 },
        576: { slidesPerView: 2 },
        768: { slidesPerView: 3 },
        1024: { slidesPerView: 4 },
        1400: { slidesPerView: 5 },
    }
});
//========= End Product Slider with Swiper =========//

//========= Start Suggest Moment =========//
let swiperSugget = new Swiper(".suggetMoment", {
    slidesPerView: 1,
    spaceBetween: 30,
    loop: true,
    autoplay: {
        delay: 3000,
        disableOnInteraction: false,
    },
    speed: 500,
    on: {
        init: function () {
            $(".swiper-progress-bar").removeClass("animate active").eq(0).addClass("animate active");
        },
        slideChangeTransitionStart: function () {
            $(".swiper-progress-bar").removeClass("animate active").eq(0).addClass("active");
        },
        slideChangeTransitionEnd: function () {
            $(".swiper-progress-bar").eq(0).addClass("animate");
        }
    }
});
//========= End Suggest Moment =========//
