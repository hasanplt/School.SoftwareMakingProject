const calendarAddForm = document.querySelector('#calendar-add-form');

calendarAddForm.addEventListener("submit", submitCalendarAdd);

async function submitCalendarAdd(e) {
    e.preventDefault();

    let formJsonData = formToJsonData(this);
    console.log(formJsonData);
    let resp = await fetchAddEvent(formJsonData.description, formJsonData.date + " " + formJsonData.time);

    if (resp.value)
        window.location.href = "/calendar";
    else
        alert(resp.message);
}

async function fetchAddEvent(description, datetime) {
    console.log(event);
    const response = await fetch('/api/event/insert?description=' + description + "&eventdatetime="+datetime, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const data = await response.json();

    return data;
}



function formToJsonData(formElement) {
    let inputs = formElement.querySelectorAll("input");
    let data = {};

    inputs.forEach(input => {
        data[input.name] = input.value;
    });

    return data;
}