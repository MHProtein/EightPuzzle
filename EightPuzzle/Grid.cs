namespace EightPuzzle
{
    public class Grid
    {
        public bool IsPossessed { get; private set; }
        public Vector2 position;
        
        public int Number
        {
            get
            {
                if(IsPossessed)
                    return number;
                return 0;
            }
            set
            {
                number = value;
                if(number == 0)
                    IsPossessed = false;
                else
                    IsPossessed = true;
            }
        }

        private int number;
        
        public Grid()
        {
            position = new Vector2();
            IsPossessed = false;
            Number = 0;
        }
        
        public Grid(Grid grid)
        {
            position = new Vector2(grid.position.x, grid.position.y);
            IsPossessed = grid.IsPossessed;
            Number = grid.Number;
        }
        
        public Grid(int number, Vector2 position)
        {
            this.position = new Vector2();
            IsPossessed = true;
            Number = number;
            this.position = position;
        }
    }
}