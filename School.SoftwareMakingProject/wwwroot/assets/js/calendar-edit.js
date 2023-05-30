const calendarEditForm = document.querySelector('#calendar-edit-form');

calendarEditForm.addEventListener("submit", submitCalendarEdit);

async function submitCalendarEdit(e) {
    e.preventDefault();

    let formJsonData = formToJsonData(this);
    console.log(formJsonData);
    let resp = await fetchEditEvent(formJsonData.description, formJsonData.date + " " + formJsonData.time, formJsonData.id);

    if (resp.value)
        window.location.href = "/calendar";
    else
        alert(resp.message);
}

async function fetchEditEvent(description, datetime, id) {

    const response = await fetch('/api/event/edit?description=' + description + "&eventdatetime=" + datetime +"&id=" + id, {
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