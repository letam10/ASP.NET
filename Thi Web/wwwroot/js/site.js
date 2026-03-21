// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    const KEY = "techshop:lastScroll";

    document.addEventListener("click", function (e) {
        const a = e.target.closest("a");
        if (!a) return;

        // Nếu click vào card danh mục
        if (a.classList.contains("cat-card")) {
            sessionStorage.setItem(KEY, String(window.scrollY));
        }
    });

    // Khi reload trang Home/Index, restore nếu có
    const isHome = (window.location.pathname === "/" || window.location.pathname.toLowerCase().includes("/home/index"));
    if (isHome) {
        const y = sessionStorage.getItem(KEY);
        if (y) {
            window.scrollTo(0, parseInt(y, 10));
            sessionStorage.removeItem(KEY);
        }
    }
})();
