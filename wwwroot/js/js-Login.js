// JavaScript Document
const container  = document.querySelector(".container"),
      pwShowHide = document.querySelectorAll(".showHidePw"),
      pwFields   = document.querySelectorAll(".password"),
      signUp     = document.querySelector(".signup-link"),
      login      = document.querySelector(".login-link"),
      informacion= document.querySelector(".informacion-link");

    // EFECTO: click en ojo y poder visualizar contraseña
    pwShowHide.forEach(eyeIcon =>{
        eyeIcon.addEventListener("click", ()=>{
            pwFields.forEach(pwField =>{
                if(pwField.type ==="password"){
                    pwField.type = "text";

                    pwShowHide.forEach(icon =>{
                        icon.classList.replace("uil-eye-slash", "uil-eye");
                    })
                }else{
                    pwField.type = "password";

                    pwShowHide.forEach(icon =>{
                        icon.classList.replace("uil-eye", "uil-eye-slash");
                    })
                }
            }) 
        })
    })

document.getElementById('email').addEventListener('input', function () {
    campo = event.target;
    valido = document.getElementById('emailOK');

    emailRegex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;
    //emailRegex = /[i][1-9]{9}^@cibertec.edu.pe$/;
    if (emailRegex.test(campo.value)) {
        valido.setAttribute("style", "color:green; font-size: 13px;text-align: end; position: absolute;");
        valido.innerText = "Correo valido"

    } else {
        valido.setAttribute("style", "color:crimson; font-size: 13px;text-align: end; position: absolute;");
        valido.innerText = "Solo se admiten correos de Cibertec";
    }
});
