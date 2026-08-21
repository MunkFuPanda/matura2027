using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventar_optimieren
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class Item
    {
        public int size;
        public int gold;

        public Item(int size, int gold) {
            this.size = size;
            this.gold = gold;
        }
    }

    public class Item2
    {
        public int size;
        public int gold;
        public int volume;

        public Item2(int size, int gold, int volume)
        {
            this.size = size;
            this.gold = gold;
            this.volume = volume;
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Item> items = new List<Item>()
            {
                new Item(41, 442),
                new Item(50, 525),
                new Item(49, 511),
                new Item(59, 593),
                new Item(55, 546),
                new Item(57, 564),
                new Item(60, 617)
            };

            List<Item2> items2 = new List<Item2>()
            {
                new Item2(41, 442, 100),
                new Item2(50, 525, 200),
                new Item2(49, 511, 300),
                new Item2(59, 593, 400),
                new Item2(55, 546, 500),
                new Item2(57, 564, 600),
                new Item2(60, 617, 700)
            };

            List<Item2> taking = new List<Item2>();

            int takingnumber = 0;

            taking = knapsackReck2chatgpt(170, 1000, items2, items.Count());

            foreach (Item2 i in taking)
            {
                MessageBox.Show(Convert.ToString(i.size), Convert.ToString(i.gold));
            }

            

            
        }

        public List<Item> knapsackReck(int size, List<Item> items, int listsize, List<Item> taking)
        {
            if (listsize == 0 || size == 0)
            {
                return null;
            }

            if (items[listsize -1].size <= size)
            {
                taking.Add(items[listsize - 1]);
                knapsackReck(size - items[listsize - 1].size, items, listsize - 1, taking);
            }

            knapsackReck(size, items, listsize - 1, taking);

            return taking;
        }

        public List<Item> knapsackReckchatgpt(int size, List<Item> items, int listsize)
        {
            if (listsize == 0 || size == 0)
                return new List<Item>();

            Item current = items[listsize - 1];

            // wenn Item zu groß -> nicht nehmen
            if (current.size > size)
                return knapsackReckchatgpt(size, items, listsize - 1);

            // Item nehmen
            List<Item> take = knapsackReckchatgpt(size - current.size, items, listsize - 1);
            take = new List<Item>(take);
            take.Add(current);

            // Item nicht nehmen
            List<Item> notTake = knapsackReckchatgpt(size, items, listsize - 1);

            int goldTake = take.Sum(i => i.gold);
            int goldNotTake = notTake.Sum(i => i.gold);

            if (goldTake > goldNotTake)
                return take;
            else
                return notTake;
        }

        public List<Item2> knapsackReck2chatgpt(int size, int volume, List<Item2> items, int listsize)
        {
            if (listsize == 0 || size == 0)
                return new List<Item2>();

            Item2 current = items[listsize - 1];

            // wenn Item zu groß -> nicht nehmen
            if (current.size > size || current.volume > volume)
                return knapsackReck2chatgpt(size, volume, items, listsize - 1);

            // Item nehmen
            List<Item2> take = knapsackReck2chatgpt(size - current.size, volume - current.volume, items, listsize - 1);
            take = new List<Item2>(take);
            take.Add(current);

            // Item nicht nehmen
            List<Item2> notTake = knapsackReck2chatgpt(size, volume, items, listsize - 1);

            int goldTake = take.Sum(i => i.gold);
            //int volumeTake = take.Sum(i => i.volume);
            int goldNotTake = notTake.Sum(i => i.gold);
            //int volumeNotTake = notTake.Sum(i => i.volume);

            if (goldTake > goldNotTake)
                return take;
            else
                return notTake;
        }
    }
}