const prevButton = document.querySelector(".btn-prev"),
    nextButton = document.querySelector(".btn-next"),
    dateText = document.querySelector(".date"),
    searchButton = document.querySelector(".btn-search"),
    todayButton = document.querySelector(".btn-today"),
    daysContainer = document.querySelector(".days"),
    dateInput = document.querySelector(".input-date"),
    dateTitle = document.querySelector(".todo-date>h1"),
    dateSubtitle = document.querySelector(".todo-date>h3"),
    eventsContainer = document.querySelector(".events");

let today = new Date(),
    selectedDay = today.getDate(),
    month = today.getMonth(),
    year = today.getFullYear(),
    events = "",
    eventsArray = [];

window.addEventListener("load", loadWindow);

const months = ["Ocak", "Subat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", 
    "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"];

const weekdays = ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"]

//const eventsArray = [
//    {
//        day: 2,
//        month: 4,
//        year: 2023,
//        events: [
//            {
//                title: "Event adı",
//                time: "10:00",
//                isComplete: false
//            },
//            {
//                title: "Event adı",
//                time: "11:00",
//                isComplete: false
//            }
//        ]
//    },
//    {
//        day: 12,
//        month: 5,
//        year: 2023,
//        events: [
//            {
//                title: "Event adı",
//                time: "10:00",
//                isComplete: true
//            },
//            {
//                title: "Event adı",
//                time: "11:00",
//                isComplete: false
//            }
//        ]
//    },
//    {
//        day: 15,
//        month: 5,
//        year: 2023,
//        events: [
//            {
//                title: "Event adı",
//                time: "10:00",
//                isComplete: false
//            },
//            {
//                title: "Event adı",
//                time: "11:00",
//                isComplete: false
//            }
//        ]
//    },
//    {
//        day: 25,
//        month: 5,
//        year: 2023,
//        events: [
//            {
//                title: "Event adı",
//                time: "10:00",
//                isComplete: false
//            },
//            {
//                title: "Event adı",
//                time: "11:00",
//                isComplete: false
//            }
//        ]
//    }
//];

async function loadWindow() {
    events = (await fetchGetEvents()).value;
    console.log("data");
    console.log(dbEventToClientEventArray(events));
    eventsArray = dbEventToClientEventArray(events);
    await initCalendar();
}
async function fetchGetEvents() {
    let data = await fetch("/api/event", {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }).then(res => res.json());

    return data;
}

function dbEventToClientEventArray(events) {
    let eventObjects = [];
    events.forEach(event => {

        let eventEndDateTime = new Date(event.event_end_datetime);
        
        for (var i = 0; i < eventObjects.length; i++) {
            let eventObject = eventObjects[i];
            console.log(eventObject);
            if (eventObject.day == eventEndDateTime.getDate() && eventObject.month == eventEndDateTime.getMonth() + 1 && eventObject.year == eventEndDateTime.getFullYear()) {
                eventObject.events.push({
                    id: event.id,
                    title: event.description,
                    time: eventEndDateTime.getHours() + ":" + eventEndDateTime.getMinutes(),
                    isComplete: event.is_complete
                });
                break;
            } else {
                let obj = {
                    day: eventEndDateTime.getDate(),
                    month: eventEndDateTime.getMonth() + 1,
                    year: eventEndDateTime.getFullYear(),
                    events: [
                        {
                            id: event.id,
                            title: event.description,
                            time: eventEndDateTime.getHours() + ":" + eventEndDateTime.getMinutes(),
                            isComplete: event.is_complete
                        }
                    ]
                };
                eventObjects.push(obj);
                break;
            }
        }
        if (eventObjects.length == 0) {
            let obj = {
                day: eventEndDateTime.getDate(),
                month: eventEndDateTime.getMonth() + 1,
                year: eventEndDateTime.getFullYear(),
                events: [
                    {
                        id: event.id,
                        title: event.description,
                        time: eventEndDateTime.getHours() + ":" + eventEndDateTime.getMinutes(),
                        isComplete: event.is_complete
                    }
                ]
            };
            eventObjects.push(obj);
        }
       

    });

    return eventObjects;
}

async function initCalendar(){
    const firstDay = new Date(year, month, 0);
    const lastDay = new Date(year, month + 1, 0);
    const prevLastDay = new Date(year, month, 0);
    const prevDays = prevLastDay.getDate();
    const lastDate = lastDay.getDate();
    const day = firstDay.getDay();
    const nextDays = 7 - lastDay.getDay();
    
    dateText.textContent = `${months[month]} ${year}`;

    let days = "";

    for(let x = day; x > 0; x--){
        days += `<div class="day prev-month" >${prevDays - x + 1}</div>`;
    }

    for(let i = 1; i <= lastDate; i++){

        let event = false;
        eventsArray.forEach((eventObject) => {
            if(i == eventObject.day &&
                year == eventObject.year &&
                month + 1 == eventObject.month){
                    let eventCounter = eventObject.events.length;
                    eventObject.events.forEach((event) => {
                        if(event.isComplete){
                            eventCounter --;
                        }
                    });
                    if(eventCounter != 0){
                        event = true;
                    }
                }
        });

        if(i == new Date().getDate() && 
        year == new Date().getFullYear() &&
        month == new Date().getMonth()){

            if(event){
                days += `<div class="day today event" >${i}</div>`;
            }
            else{
                days += `<div class="day today" >${i}</div>`;
            }
            await updateEvents(i);
            getActiveDay(today.getDate());

        }
        else{
            if(event){
                days += `<div class="day event" >${i}</div>`;
            }
            else{
                days += `<div class="day" >${i}</div>`;
            }
        }
    }

    for(let j = 1; j <= nextDays; j++){
        days += `<div class="day next-month" >${j}</div>`;
    }

    daysContainer.innerHTML = days;
    await addListener();
}


async function prevMonth(){
    month--;
    if(month < 0){
        month = 11;
        year--;
    }
    await initCalendar();
}

async function nextMonth(){
    month++;
    if(month > 11){
        month = 0;
        year++;
    }
    await initCalendar();
}

prevButton.addEventListener("click", prevMonth);
nextButton.addEventListener("click", nextMonth);

todayButton.addEventListener("click", async () => {
    today = new Date();
    month = today.getMonth();
    year = today.getFullYear();
    await initCalendar();
    await updateEvents(today.getDate());
    getActiveDay(today.getDate());
});

searchButton.addEventListener("click", async function(){
    let dateSearch = dateInput.value.split("-");
    if(parseInt(dateSearch[0]) > 1800 &&
    parseInt(dateSearch[1]) < 13 &&
    parseInt(dateSearch[2]) < 32){
        month = dateSearch[1] - 1;
        year = dateSearch[0];
        await initCalendar();

        setTimeout(() => {

            const days = document.querySelectorAll(".day");
            days.forEach((day) => {
                if(!day.classList.contains("prev-month")&&
                !day.classList.contains("next-month")&&
                day.innerHTML == dateSearch[2]){
                    day.classList.add("selected");
                    getActiveDay(dateSearch[2]);
                }
            });
        }, 100);

        return;
    }
    alert("Geçersiz tarih girişi. Tekrar deneyin.");
});

async function addListener() {
    const days = document.querySelectorAll(".day");
    days.forEach((day) => {
        day.addEventListener("click", async (e) => {
            selectedDay = Number(e.target.innerHTML);
            getActiveDay(e.target.innerHTML);
            await updateEvents(e.target.innerHTML);
            
            days.forEach((day) => {
                day.classList.remove("selected");
            });

            if(e.target.classList.contains("prev-month")){
                prevMonth();

                setTimeout(() => {

                    const days = document.querySelectorAll(".day");
                    days.forEach((day) => {
                        if(!day.classList.contains("prev-month")&&
                        day.innerHTML == e.target.innerHTML){
                            day.classList.add("selected");
                        }
                    });
                }, 100);
            }
            else if(e.target.classList.contains("next-month")){
                nextMonth();

                setTimeout(() => {

                    const days = document.querySelectorAll(".day");
                    days.forEach((day) => {
                        if(!day.classList.contains("next-month")&&
                        day.innerHTML == e.target.innerHTML){
                            day.classList.add("selected");
                        }
                    });
                }, 100);
            }
            else{
                const days = document.querySelectorAll(".day");
                days.forEach((day) => {
                    if(!day.classList.contains("prev-month")&&
                    !day.classList.contains("next-month")&&
                    day.innerHTML == e.target.innerHTML){
                        day.classList.add("selected");
                    }
                })
            }
        });
    });
}

function getActiveDay(date){
    const day = new Date(year, month, date);
    selectedDay = date;
    dateTitle.innerHTML = weekdays[day.getDay()];
    dateSubtitle.innerHTML = `${date} ${months[month]} ${year}`;
}

async function updateEvents(date){
    let events= "";
    eventsArray.forEach((event) => {
        if(
            date == event.day &&
            month + 1 == event.month &&
            year == event.year
        ) {
            event.events.forEach((event) => {
                events += `
                <div class="event ${event.isComplete ? "complete" : ""}" data-id="${event.id}">
                    <div class="event-title">${event.title}</div>
                    <div class="event-time">${event.time}</div>
                    <span class="${event.isComplete ? "replay" : "check"}"></span>
                    <a href="/calendar/edit/${event.id}" class="edit"></a>
                    <span class="cancel"></span>
                </div>
                `;
            });
        }
    });

    if((events == "")) {
        events = `<h3>Etkinlik yok</h3>`;
    }

    eventsContainer.innerHTML = events;
    await buttonsEvent();
}

async function buttonsEvent(){
    let events = document.querySelectorAll(".event");
    events.forEach(async (eventNode) => {
        eventNode.addEventListener("click",async (e) => {
            if(e.target.classList.contains("check")){
                eventsArray.forEach(async (event) => {
                    if(
                        selectedDay == event.day &&
                        month + 1 == event.month &&
                        year == event.year
                    ) {
                        var isSuccessUpdate = await fetchUpdateEvent(eventNode.dataset.id, "true");
                        if (isSuccessUpdate.value) {
                            event.events.forEach((event) => {
                                if (event.title == eventNode.children[0].innerHTML &&
                                    event.time == eventNode.children[1].innerHTML &&
                                    event.isComplete == eventNode.classList.contains("complete")) {
                                    e.target.classList.remove("check");
                                    e.target.classList.add("replay");
                                    eventNode.classList.add("complete");
                                    event.isComplete = true;
                                }
                            });
                        } else {
                            alert(isSuccessUpdate.message);
                        }
                    }
                    await initCalendar();
                });
            }
            else if(e.target.classList.contains("replay")){
                eventsArray.forEach(async (event) => {
                    if(
                        selectedDay == event.day &&
                        month + 1 == event.month &&
                        year == event.year
                    ) {
                        var isSuccessUpdate = await fetchUpdateEvent(eventNode.dataset.id, "false");
                        if (isSuccessUpdate.value) {
                            event.events.filter((event) => {
                                if (event.title == eventNode.children[0].innerHTML &&
                                    event.time == eventNode.children[1].innerHTML &&
                                    event.isComplete == eventNode.classList.contains("complete")) {
                                    e.target.classList.add("check");
                                    e.target.classList.remove("replay");
                                    eventNode.classList.remove("complete");
                                    event.isComplete = false;
                                }
                            });
                        } else {
                            alert(isSuccessUpdate.message);
                        }
                    }
                    await initCalendar();
                });
            }
            else if (e.target.classList.contains("edit")) {
                //window.location.href = "/event/edit/" + eventNode.dataset.id;
            }
            else if (e.target.classList.contains("cancel")) {
                eventsArray.forEach(async (event) => {
                    if(
                        selectedDay == event.day &&
                        month + 1 == event.month &&
                        year == event.year
                    ) {
                        console.log(event.events);
                        let isSuccess = await fetchDeleteEvent(eventNode.dataset.id);
                        if (isSuccess) {
                            event.events = event.events.filter((event) => {
                                if (event.title == eventNode.children[0].innerHTML &&
                                    event.time == eventNode.children[1].innerHTML &&
                                    event.isComplete == eventNode.classList.contains("complete")) {
                                    eventNode.remove();
                                }
                                else {
                                    return event;
                                }
                            });
                           
                        }
                       
                        
                        console.log(event.events);
                    }
                    await initCalendar();
                });
            }
        });
    })
}

async function fetchDeleteEvent(eventId) {
    let res = await fetch("/api/event/" + eventId, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
        }
    }).then(response => response.json());

    return res;
}

async function fetchUpdateEvent(id, is_complete) {
    let res = await fetch("/api/event/update?id="+id+"&is_complete="+is_complete, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        }
    }).then(response => response.json());
    console.log(res);
    return res;
}
/**
 * events += `
                            <div class="event ${event.isComplete ? "complete" : ""}">
                                <div class="event-title">${event.title}</div>
                                <div class="event-time">${event.time}</div>
                                <span class="${event.isComplete ? "replay" : "check"}"></span>
                                <span class="edit"></span>
                                <span class="cancel"></span>
                            </div>
                            `;
 */