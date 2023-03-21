export class OrdersModel {
    id: number = 0;
    index: string = '';
    name: string = '';
    filePath: string = '';
    goodsName: string = '';
    customersName: string = '';
    NickName: string = '';
    price: number = 0;
    num: number = 0;
    sum: number = this.num*this.price;
    order: number = 99;
    isEnable: boolean = false;
    description: string = "";
}

