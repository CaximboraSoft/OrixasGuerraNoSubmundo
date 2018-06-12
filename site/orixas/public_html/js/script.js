function alterna(x){
    if(x===1){
        document.getElementById("home").style.display="block";
        document.getElementById("jogos").style.display="none";
        document.getElementById("sobre").style.display="none";
        document.getElementById("contato").style.display="none";
    }else if(x===2){
        document.getElementById("home").style.display="none";
        document.getElementById("jogos").style.display="block";
        document.getElementById("sobre").style.display="none";
        document.getElementById("contato").style.display="none";
    }else if(x===3){
        document.getElementById("home").style.display="none";
        document.getElementById("jogos").style.display="none";
        document.getElementById("sobre").style.display="block";
        document.getElementById("contato").style.display="none";
    }else if(x===4){
        document.getElementById("home").style.display="none";
        document.getElementById("jogos").style.display="none";
        document.getElementById("sobre").style.display="none";
        document.getElementById("contato").style.display="block";
    }
}