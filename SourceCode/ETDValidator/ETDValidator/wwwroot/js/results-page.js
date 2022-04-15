// any components that are added for the validation UI can be added in this file
//document
class Queue {
    constructor() {
        this.elements = {};
        this.head = 0;
        this.tail = 0;
    }
    enqueue(element) {
        this.elements[this.tail] = element;
        this.tail++;
    }
    dequeue() {
        const item = this.elements[this.head];
        delete this.elements[this.head];
        this.head++;
        return item;
    }
    peek() {
        return this.elements[this.head];
    }
    get length() {
        return this.tail - this.head;
    }
    get isEmpty() {
        return this.length === 0;
    }
}

let keyQueue = new Queue();
let valueQueue = new Queue();
let documentName = null;
let documentWarnings = [];
let documentErrors = [];

// function is entry point into file from results view
function buildValidationUI(jsonString) {
    // populate the two queues that will hold keys and values associated with the keys
    parseObjectKeys(jsonString);

    // Populate the documentName and appropiate warnings and errors
    populateDocumentData()

}

// Populate the documentName and appropiate warnings and errors
function populateDocumentData() {
    // now with the populated FIFO queues iterate through and get appropiate data
    while (!keyQueue.isEmpty){
        // Get the first key value
        const currentKey = keyQueue.peek();
        const currentValue = valueQueue.peek();

        switch (currentKey) {
            case("document_name"):
                documentName = currentValue;
                break;

            case ("warnings"): {
                if (currentValue.length !== 0) {
                    Object.values(currentValue).forEach(warning => {
                        documentWarnings.push(warning);
                    });
                }
                break;
            }
            case ("errors"):{
                if (currentValue.length !== 0) {
                    currentValue.forEach(error => {
                        documentErrors.push(currentValue.valueOf(error));
                    });
                }
                break;
            }
            default:
                
                break;
        }
        keyQueue.dequeue();
        valueQueue.dequeue();
    }
}



// Function to recursively collect all of the keys 
function parseObjectKeys(obj) {

    for (const prop in obj) {
        //console.log(prop)
        keyQueue.enqueue(prop)
        valueQueue.enqueue(obj[prop])
        const sub = obj[prop];
        if (typeof(sub) == "object") {
            parseObjectKeys(sub);
        }
    }
}
