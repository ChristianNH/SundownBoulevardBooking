var BookingApp = new Vue({
    el: '#BookingApp',
    data: {
        Name: "ChristianTest",
        Email: "sectah@gmail.com",
        Phone: "50851706",
        PartySize: 2,
        SelectedDate: "",
        SelectedTime: "16:00",
        Loading: false,
        TimeslotAvailable: false,
        TimeslotError: false,
        Drinks: null,
        DrinksPaging: 1,
        SelectedDrink: null,
        Dishes: null,
        DishesPaging: 1,
        SelectedDish: null,
        Step: ""
    },
    watch: {
        SelectedTime: function (newVal, oldVal) { // watch it
            this.TimeslotError = false;
            this.TimeslotAvailable = false;
            this.Step = "";
        },
        SelectedDate: function (newVal, oldVal) { // watch it
            this.TimeslotError = false;
            this.TimeslotAvailable = false;
            this.Step = "";
        },
        SelectedDrink: function (newVal, oldVal) { // watch it
            this.GetDishes();
        },
        SelectedDish: function (newVal, oldVal) { // watch it
            this.Step = "confirmbooking";
        },
        PartySize: function (newVal, oldVal) { // watch it
            this.TimeslotError = false;
            this.TimeslotAvailable = false;
            this.Step = "";
        },
        TimeslotAvailable: function (newVal, oldVal) { // watch it
            if (this.TimeslotAvailable == true) {
                this.GetDrinks();
                this.Step = "information";
            }
        },
        DrinksPaging: function (newVal, oldVal) { // watch it
            this.GetDrinks();
        }
    },
    mounted: function () {
        var that = this;
        this.GetTables();
        
        //Init date selector
        $("#dateSelector").flatpickr(
            {
                dateFormat: "D j F",
                defaultDate: "today",
                //"locale": "da",
                onChange: function (selectedDates, dateStr, instance) {
                  
                    var d = new Date(Date.parse(selectedDates[0]));
                    var ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d);
                    var mo = new Intl.DateTimeFormat('en', { month: '2-digit' }).format(d);
                    var da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d);
                    var formattedDate = ye + "-" + mo + "-" + da;

                    that.SelectedDate = formattedDate;
                }
            }
        );
    },
    methods: {
        GetTables: GetTables,
        CheckTimeslot: CheckTimeslot,
        BookTable: BookTable,
        GetDrinks: GetDrinks,
        GetDishes: GetDishes,
        GetDrinksPagingIncrease: function () {
            
            this.DrinksPaging++;
        },
        GetDrinksPagingDecrease: function () {
            if (this.DrinksPaging == 1) {
                return;
            }
            this.DrinksPaging--;
        },
    },
});
function GetDrinks() {
    var that = this;
    $.ajax({
        url: "https://api.punkapi.com/v2/beers?per_page=6&page=" + that.DrinksPaging,
        dataType: 'json',
        type: "GET",
        error: requestError,
        success: getDrinksSucces
    });
}
function getDrinksSucces(resp) {
    console.log(resp);
    BookingApp.Drinks = resp;

}
function GetDishes() {
    var that = this;
    $.ajax({
        url: "https://www.themealdb.com/api/json/v1/1/filter.php?c=Seafood",
        dataType: 'json',
        type: "GET",
        error: requestError,
        success: getDishesSucces
    });
}
function getDishesSucces(resp) {
    console.log(resp);
    BookingApp.Dishes = resp.meals;

}

function GetTables() {
    $.ajax({
        url: "/jsonservice.aspx?f=gettables",
        dataType: 'json',
        type: "GET",
        error: requestError,
        success: getTablesSucces
    });
    
}
function getTablesSucces(resp) {
    console.log(resp);
    
}

function CheckTimeslot() {
    var that = this;
    var Timeslot = {
        SelectedDate: that.SelectedDate + " " + that.SelectedTime,
        PartySize: that.PartySize
    };

    $.ajax({
        url: "/jsonservice.aspx?f=checktimeslot",
        dataType: 'json',
        data: Timeslot,
        type: "POST",
        error: requestError,
        success: CheckTimeslotSucces
    });
}
function CheckTimeslotSucces(resp) {
    console.log(resp);
    
    if (resp === "True") {
        BookingApp.TimeslotAvailable = true;
        BookingApp.TimeslotError = false;
        BookingApp.Step = "information";
    } else {
        BookingApp.TimeslotAvailable = false;
        BookingApp.TimeslotError = true;
    }
}

function BookTable() {
    var that = this;

    var Booking = {
        Name: that.Name,
        Email: that.Email,
        Phone: that.Phone,
        SelectedDate: that.SelectedDate + " " + that.SelectedTime,
        PartySize: that.PartySize
    };

    $.ajax({
        url: "/jsonservice.aspx?f=booktable",
        dataType: 'json',
        type: "POST",
        data: Booking,
        error: requestError,
        success: bookTableSucces
    });
}

function bookTableSucces(resp) {
    console.log(resp);
}

function requestError(error)
{
    //error
    console.log(error);
}

