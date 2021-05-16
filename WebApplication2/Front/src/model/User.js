export default class User {
    constructor(
        json
    ) {
        this.id = json.id
        this.name = json.name
        this.userCategory = json.userCategory
        this.barcode = json.barcode
        this.rfid = json.rfid
        this.canBorrowBooks = json.canBorrowBooks
        this.registrationDate = new Date(json.registrationDate)
    }
}