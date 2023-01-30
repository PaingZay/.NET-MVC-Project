const CustomerId = 11;
var ProductId = document.getElementById("productId").value;

function ChangeStyle()
{
    ProductId = document.getElementById("productId").value;
}

window.onload = function () {
    
    RestoreStarsDB();
    ListenStars();
}

function ListenStars() {
    let star = document.getElementsByClassName("star");
    for (let i = 0; i < star.length; i++) {
        star[i].addEventListener('click', onClickStars);
    }
}



function RestoreStarsDB() {
    RetrieveStarsDB();
}

function RetrieveStarsDB() {
    let ajax = new XMLHttpRequest();
    ajax.open("GET", "/Rating/GetStar?pid=" + ProductId);

    ajax.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                onRetrieveStars(JSON.parse(this.responseText));
            }
        }
    }
    ajax.send();
}

function onRetrieveStars(data) {
    if (data == null) {
        return;
    }

    let starR = document.getElementById(data.Rating);
    if (starR != null) {
        starR.checked = true;
    }
}

function onClickStars(event) {
    console.log(ProductId);
    SetRatingStars(CustomerId, ProductId, event.target.id);
    
}

function SetRatingStars(UserId, ProductId, Rating)
{
    let ajax = new XMLHttpRequest();

    ajax.open("POST", "/Rating/SetStarRating");
    ajax.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    ajax.onreadystatechange = function ()
    {
        if (this.readyState == XMLHttpRequest.DONE)
        {
            if (this.status == 200)
            {
                return this.responseText;
            }
        }
    }

    // Send the value pair to RatingController
    ajax.send("UserId=" + UserId + "&ProductId=" + ProductId + "&Rating=" + Rating);
}
