const eyeIcon = document.querySelector(".show-hide");

eyeIcon.addEventListener('click', () => {
  const pInput = eyeIcon.parentElement.querySelector("input");
  if(pInput.type === "password" ){
    pInput.type = "text";
    eyeIcon.textContent = "visibility_off";
  }else{
     pInput.type = "password";
     eyeIcon.textContent = "visibility";
  }
  
});