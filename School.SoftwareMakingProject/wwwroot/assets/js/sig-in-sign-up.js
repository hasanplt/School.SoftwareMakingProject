const toggleButton = document.querySelector(".btn-toggle"),
    signInButton = document.querySelector(".btn-sign-in"),
    signUpButton = document.querySelector(".btn-sign-up"),
    signInForm = document.querySelector(".sign-in"),
    signUpForm = document.querySelector(".sign-up");

signInButton.addEventListener("click", swipeLeft);
signUpButton.addEventListener("click", swipeRight);
signUpForm.addEventListener("submit", signUpUser);
signInForm.addEventListener("submit", signInUser);


function swipeLeft(){
    toggleButton.classList.remove("right");
    toggleButton.classList.add("left");
    toggleButton.style.left = "0px";
    toggleButton.innerHTML = "Giriş Yap";
    signInForm.style.transform = "translateX(0%)";
    signUpForm.style.transform = "translateX(200%)";
    document.title = "Giriş Yap - sesQ";
}

function swipeRight(){
    toggleButton.classList.remove("left");
    toggleButton.classList.add("right");
    toggleButton.style.left = "113px";
    toggleButton.innerHTML = "Üye Ol";
    signInForm.style.transform = "translateX(-200%)";
    signUpForm.style.transform = "translateX(0%)";
    document.title = "Üye Ol - sesQ";
}

async function signUpUser(e) {
    e.preventDefault();
    let response = await fetchDataToRegister(formToJsonData(this));

    if (response.value) {
        clearDatasForm(this);
        swipeLeft();
    }
    alert(response.message);
}

async function fetchDataToRegister(data) {
    let resp = await fetch("/api/auth/register", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: data,
    }).then(res => res.json());

    return resp;
}

async function signInUser(e) {
    e.preventDefault();
    let response = await fetchDataToLogin(formToJsonData(this));

    if (response.value) {
        clearDatasForm(this);
    }
    alert(response.message);

    setTimeout(() => {
        window.location.href = "/calendar";
    }, 1000);

    console.log(response);
}
async function fetchDataToLogin(data) {
    let resp = await fetch("/api/auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: data,
    }).then(res => res.json());

    return resp;
}

function clearDatasForm(formElement) {
    let inputs = formElement.querySelectorAll("input");

    inputs.forEach(input => {
        input.value = "";
    });

}

function formToJsonData(formElement) {
    let inputs = formElement.querySelectorAll("input");
    let data = {};

    inputs.forEach(input => {
        data[input.name] = input.value;
    });

    return JSON.stringify(data);
}