gsap.utils.toArray(".reveal").forEach((el) => {
  gsap.from(el, {
    opacity: 0,
    y: 40,
    duration: 1,
    scrollTrigger: { trigger: el, start: "top 85%" },
  });
});

const buttons = document.querySelectorAll(".region-btn");
const contents = document.querySelectorAll(".network-content");

const stats = {
  north: { branches: "32+", offices: "4" },
  west: { branches: "28+", offices: "4" },
  east: { branches: "20+", offices: "4" },
  south: { branches: "30+", offices: "4" },
};

buttons.forEach((btn) => {
  btn.addEventListener("click", () => {
    buttons.forEach((b) => b.classList.remove("active"));
    btn.classList.add("active");
    contents.forEach((c) => c.classList.remove("active"));
    document.getElementById(btn.dataset.region).classList.add("active");

    document.getElementById("statBranches").innerText =
      stats[btn.dataset.region].branches;
    document.getElementById("statOffices").innerText =
      stats[btn.dataset.region].offices;
  });
});

let map;
document.querySelectorAll(".map-btn").forEach((btn) => {
  btn.addEventListener("click", () => {
    const lat = btn.dataset.lat;
    const lng = btn.dataset.lng;
    const modal = new bootstrap.Modal(document.getElementById("mapModal"));
    modal.show();
    setTimeout(() => {
      if (map) map.remove();
      map = L.map("map").setView([lat, lng], 13);
      L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png").addTo(
        map
      );
      L.marker([lat, lng]).addTo(map);
    }, 300);
  });
});

// GSAP ScrollTrigger for stats counter
gsap.registerPlugin(ScrollTrigger);

// Animate only elements with .stat-count
gsap.utils.toArray(".stat-count").forEach((stat) => {
  const endValue = parseInt(stat.textContent.replace(/\D/g, ""));
  stat.textContent = "0";
  gsap.to(stat, {
    innerText: endValue,
    duration: 2.2,
    ease: "power1.out",
    snap: { innerText: 1 },
    scrollTrigger: {
      trigger: stat,
      start: "top 80%",
      toggleActions: "play none none none",
    },
    onUpdate: function () {
      stat.textContent = Math.floor(stat.innerText);
      if (stat.id === "statStaff") stat.textContent += "K+";
      if (stat.id === "statBranches") stat.textContent += "+";
    },
  });
});
