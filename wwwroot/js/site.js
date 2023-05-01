window.onload = async () => {

    //  http://localhost:5004/api/TodoItmes/xml
    const response = await fetch("http://localhost:5004/api/TodoItems/xml");
    const data = await response.text();

    let parser = new DOMParser();
    let xmlDoc = parser.parseFromString(data, "text/xml");

    let titles = xmlDoc.getElementsByTagName("TodoItem");

    for (const el of titles) {
        console.log(el);

    }
}