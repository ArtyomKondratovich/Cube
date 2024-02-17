class DateOnly {
    private readonly date: Date;
  
    constructor(year: number, month: number, day: number) {
      this.date = new Date(year, month - 1, day);
    }
  
    getYear(): number {
      return this.date.getFullYear();
    }
  
    getMonth(): number {
      return this.date.getMonth() + 1;
    }
  
    getDay(): number {
      return this.date.getDate();
    }
  
    toString(): string {
      const year = this.getYear();
      const month = this.getMonth().toString().padStart(2, '0');
      const day = this.getDay().toString().padStart(2, '0');
      return `${year}-${month}-${day}`;
    }
}