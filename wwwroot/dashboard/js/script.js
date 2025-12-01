const toggleDropdown = (dropdown, menu, isOpen) => {
  dropdown.classList.toggle("open", isOpen);
  menu.style.height = isOpen ? `${menu.scrollHeight}px` : 0;
};

const closeAllDropdowns = () => {
  document.querySelectorAll(".drop-container.open").forEach((openDropdown) => {
    toggleDropdown(
      openDropdown,
      openDropdown.querySelector(".drop-menu"),
      false
    );
  });
};

document.querySelectorAll(".drop-toggle").forEach((dropToggle) => {
  dropToggle.addEventListener("click", (e) => {
    e.preventDefault();

    const dropdown = e.target.closest(".drop-container");
    const menu = dropdown.querySelector(".drop-menu");

    const isOpen = dropdown.classList.contains("open");
    closeAllDropdowns();
    toggleDropdown(dropdown, menu, !isOpen);
  });
});

document.querySelectorAll(".sidebar-toggler,.header-left .header-button")
  .forEach((button) => {
    button.addEventListener("click", () => {
      closeAllDropdowns();
      document.querySelector(".sidebar").classList.toggle("collapsed");
      document.querySelector(".header").classList.toggle("collapsed");
    });
  });


if(window.innerWidth <= 1024){ 
  document.querySelector(".sidebar").classList.toggle("collapsed");
  document.querySelector(".header").classList.toggle("collapsed")

};
