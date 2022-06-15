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

    /*js code to appear signup and login form*/ 
    signUp.addEventListener("click", ( )=>{
        container.classList.add("active");
    });
    informacion.addEventListener("click", ( )=>{
        container.classList.remove("active");
    });



let boton1 = document.getElementById("btn_continuar")
let cajasTexto = document.querySelectorAll(".cajasTexto")

cajasTexto[4].addEventListener("keyup", () => {
    if (cajasTexto[0].value != "" && cajasTexto[1].value != "" && cajasTexto[2].value != "" && cajasTexto[3].value != "" && cajasTexto[4].value != "" && document.getElementById("clave").value == document.getElementById("clave_validacion").value) {
        document.getElementById("btn_continuar").disabled = false
        document.getElementById("btn_continuar").setAttribute("style", "background-color: #fc655d;")
    } else {
        document.getElementById("btn_continuar").disabled = true
        document.getElementById("btn_continuar").setAttribute("style", "background-color :silver;")
    }
})
cajasTexto[3].addEventListener("keyup", () => {
    if (cajasTexto[0].value != "" && cajasTexto[1].value != "" && cajasTexto[2].value != "" && cajasTexto[3].value != "" && cajasTexto[4].value != "" && document.getElementById("clave").value == document.getElementById("clave_validacion").value) {
        document.getElementById("btn_continuar").disabled = false
        document.getElementById("btn_continuar").setAttribute("style", "background-color: #fc655d;")
    } else {
        document.getElementById("btn_continuar").disabled = true
        document.getElementById("btn_continuar").setAttribute("style", "background-color :silver;")
    }
})
cajasTexto[2].addEventListener("keyup", () => {
    if (cajasTexto[0].value != "" && cajasTexto[1].value != "" && cajasTexto[2].value != "" && cajasTexto[3].value != "" && cajasTexto[4].value != "" && document.getElementById("clave").value == document.getElementById("clave_validacion").value) {
        document.getElementById("btn_continuar").disabled = false
        document.getElementById("btn_continuar").setAttribute("style", "background-color: #fc655d;")
    } else {
        document.getElementById("btn_continuar").disabled = true
        document.getElementById("btn_continuar").setAttribute("style", "background-color :silver;")
    }
})
cajasTexto[1].addEventListener("keyup", () => {
    if (cajasTexto[0].value != "" && cajasTexto[1].value != "" && cajasTexto[2].value != "" && cajasTexto[3].value != "" && cajasTexto[4].value != "" && document.getElementById("clave").value == document.getElementById("clave_validacion").value) {
        document.getElementById("btn_continuar").disabled = false
        document.getElementById("btn_continuar").setAttribute("style", "background-color: #fc655d;")
    } else {
        document.getElementById("btn_continuar").disabled = true
        document.getElementById("btn_continuar").setAttribute("style", "background-color :silver;")
    }
})
cajasTexto[0].addEventListener("keyup", () => {
    if (cajasTexto[0].value != "" && cajasTexto[1].value != "" && cajasTexto[2].value != "" && cajasTexto[3].value != "" && cajasTexto[4].value != "" && document.getElementById("clave").value == document.getElementById("clave_validacion").value) {
        document.getElementById("btn_continuar").disabled = false
        document.getElementById("btn_continuar").setAttribute("style", "background-color: #fc655d;")
    } else {
        document.getElementById("btn_continuar").disabled = true
        document.getElementById("btn_continuar").setAttribute("style", "background-color :silver;")
    }
})


document.getElementById("clave").addEventListener("keyup", () => {
    if (document.getElementById("clave").value != document.getElementById("clave_validacion").value ) {
        document.getElementById("msj_clave").innerHTML = "<span>Las contraseñas no coinciden</span>"
    } else {
        document.getElementById("msj_clave").innerHTML="<span>Correcto</span>"
    }
})
document.getElementById("clave_validacion").addEventListener("keyup", () => {
    if (document.getElementById("clave").value != document.getElementById("clave_validacion").value) {
        document.getElementById("msj_clave").innerHTML = "<span>Las contraseñas no coinciden</span>"
    } else {
        document.getElementById("msj_clave").innerHTML="<span>Correcto</span>"
    }
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
