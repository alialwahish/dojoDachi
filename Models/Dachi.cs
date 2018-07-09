namespace DojoDachi{

    public class Dachi{

        public int Fullness { get;set;}
        public int Happiness { get;set;}
        public int Energy { get;set;}

        public int meal {get;set;}    

        public Dachi(int ful=20,int hap=20,int eng=50,int meal=0){
            Fullness=ful;
            Happiness=hap;
            Energy=eng;
            this.meal=meal;
        }    

    }
    

    
}