function changeGreeting() {
    let greetingOptions = document.querySelectorAll('input.greeting-option');
    let greetingTextInput = document.querySelector('input.greeting-text');
    let currentGreeting = greetingTextInput.value;
    let optionFound = false;
    let newGreeting = currentGreeting;    

    for (const x of greetingOptions) {
        let optionText = x.value;
        if (optionFound) {
            newGreeting = optionText;            
            break;
        } else if (currentGreeting == optionText) {
            optionFound = true;                
        }
    };

    if (!optionFound || newGreeting == currentGreeting) {
        newGreeting = greetingOptions[0].value;
    }

    greetingTextInput.value = newGreeting;

    let greetingTextLabels = document.querySelectorAll('label.greeting-text');
    greetingTextLabels.forEach(function (x) {
        x.innerText = newGreeting;
    });
}