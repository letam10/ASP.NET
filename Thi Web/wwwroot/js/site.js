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

// Global handler: nếu AJAX gặp 401 thì đưa về Login có returnUrl
(function () {
    if (window.jQuery) {
        $(document).ajaxError(function (_event, jqxhr) {
            if (jqxhr && jqxhr.status === 401) {
                const returnUrl = encodeURIComponent(
                    window.location.pathname + window.location.search + window.location.hash
                );
                window.location.href = `/Account/Login?returnUrl=${returnUrl}`;
            }
        });
    }
})();

$(document).on("click", ".btn-wishlist", function () {
    const id = $(this).data("id");
    $.post("/Product/ToggleWishlist", { productId: id })
        .done(function (res) {
            // res là JSON khi đã login
            alert(res.message || "Đã cập nhật.");
        })
        .fail(function (xhr) {
            if (xhr.status === 401) {
                window.location.href = "/Account/Login";
            }
        });
});

(function () {
    const THEME_KEY = "techshop-theme";
    const root = document.documentElement;

    function updateThemeButton(theme) {
        const icon = document.getElementById("themeToggleIcon");
        const text = document.getElementById("themeToggleText");
        if (!icon || !text) return;

        if (theme === "dark") {
            icon.className = "bi bi-sun-fill";
            text.textContent = "Light";
        } else {
            icon.className = "bi bi-moon-stars-fill";
            text.textContent = "Dark";
        }
    }

    function applyTheme(theme) {
        root.setAttribute("data-theme", theme);
        localStorage.setItem(THEME_KEY, theme);
        updateThemeButton(theme);
    }

    document.addEventListener("DOMContentLoaded", function () {
        const initialTheme = root.getAttribute("data-theme") || localStorage.getItem(THEME_KEY) || "light";
        applyTheme(initialTheme);

        const btn = document.getElementById("themeToggleBtn");
        if (!btn) return;

        btn.addEventListener("click", function () {
            const currentTheme = root.getAttribute("data-theme") || "light";
            applyTheme(currentTheme === "dark" ? "light" : "dark");
        });
    });
})();
