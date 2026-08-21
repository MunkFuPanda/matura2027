using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel;
using System.Threading;
using System.ComponentModel;

namespace Sorting
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ObservableCollection<Int32> sortList = new ObservableCollection<Int32>();
        int _checks = 0;
        int _swaps = 0;
        int _selected = -1;

        public ObservableCollection<Int32> List
        {
            set
            {
                sortList = value;
                NotifyPropertyChanged(x => x.List);
            }
            get
            {
                return sortList;
            }
        }
        public int Checks {
            set
            {
                _checks = value;
                NotifyPropertyChanged(x => x.Checks);
            }
            get
            {
                return _checks;
            }
        }
        public int Swaps
        {
            set
            {
                _swaps = value;
                NotifyPropertyChanged(x => x.Swaps);
            }
            get
            {
                return _swaps;
            }
        }

        public int Selected
        {
            set
            {
                _selected = value;
                NotifyPropertyChanged(x => x.Selected);
            }
            get
            {
                return _selected;
            }
        }
        Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 50; i++)
            {
                sortList.Add(rand.Next(200));
            }
            Checks = 0;
            Swaps = 0;
            this.DataContext = this;
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            int size = sortList.Count;
            Checks = 0;
            Swaps = 0;
            ThreadPool.QueueUserWorkItem(o =>
            {
                bool swapped = false;
                do{
                    swapped = false;
                    for (int i = 0; i < size - 1; ++i, Selected = i)
                    {
                        try
                        {
                            this.Dispatcher.Invoke(
                              System.Windows.Threading.DispatcherPriority.Normal
                              , new System.Windows.Threading.DispatcherOperationCallback(delegate
                              {
                                  Checks++;
                                  if (sortList[i] > sortList[i + 1])
                                  {
                                      Swaps++;
                                      int temp = sortList[i];
                                      sortList[i] = sortList[i + 1];
                                      sortList[i + 1] = temp;
                                      swapped = true;
                                  }
                                  return null;
                              }), null);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.ToString());
                        }
                        Thread.Sleep(50);
                    }
                    size = size-1;
              } while (swapped == true);

            });
        }

        private void reverse_Click(object sender, RoutedEventArgs e)
        {
            int size = sortList.Count;
            Checks = 0;
            Swaps = 0;
            ThreadPool.QueueUserWorkItem(o =>
            {
                bool swapped = false;
                do
                {
                    swapped = false;
                    for (int i = 0; i < size - 1; ++i, Selected = i)
                    {
                        try
                        {
                            this.Dispatcher.Invoke(
                              System.Windows.Threading.DispatcherPriority.Normal
                              , new System.Windows.Threading.DispatcherOperationCallback(delegate
                              {
                                  Checks++;
                                  if (sortList[i] < sortList[i + 1])
                                  {
                                      Swaps++;
                                      int temp = sortList[i];
                                      sortList[i] = sortList[i + 1];
                                      sortList[i + 1] = temp;
                                      swapped = true;
                                  }
                                  return null;
                              }), null);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.ToString());
                        }
                        Thread.Sleep(50);
                    }
                    size = size - 1;
                } while (swapped == true);

            });
        }


        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged<TValue>
                     (System.Linq.Expressions.Expression<Func<MainWindow, TValue>> propertySelector)
        {
            if (PropertyChanged != null)
            {
                var memberExpression = propertySelector.Body as System.Linq.Expressions.MemberExpression;
                if (memberExpression != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
                }
            }
        }
        #endregion

        private void cocktail_Click(object sender, RoutedEventArgs e)
        {
            int size = sortList.Count;
            Checks = 0;
            Swaps = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
            int start = 0;
            int end = size;

            bool swapped = true;

                while (swapped == true)
                {
                    swapped = false;

                    // loop from bottom to top same as
                    // the bubble sort
                    for (int i = start; i < end - 1; ++i, Selected = i)
                    {
                        Checks++;
                        if (sortList[i] > sortList[i + 1])
                        {
                            Swaps++;

                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                int temp = sortList[i];
                                sortList[i] = sortList[i + 1];
                                sortList[i + 1] = temp;
                                swapped = true;
                            }));


                        }
                        Thread.Sleep(50);


                    }

                    if (swapped == false)
                        break;

                    swapped = false;

                    end = end - 1;


                    for (int i = end - 1; i >= start; i--, Selected = i)
                    {
                        Checks++;
                        if (sortList[i] > sortList[i + 1])
                        {
                            Swaps++;
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                int temp = sortList[i];
                                sortList[i] = sortList[i + 1];
                                sortList[i + 1] = temp;
                                swapped = true;
                            }));
                        }
                        Thread.Sleep(50);

                    }

                    start = start + 1;
                    
                }
            });

            
            }

        private void selection_Click(object sender, RoutedEventArgs e)
        {
            int size = sortList.Count;
            Checks = 0;
            Swaps = 0;

            ThreadPool.QueueUserWorkItem(o => 
            {
                for (int i = 0; i < size - 1; i++)
                {

                    
                    int min_idx = i;

                    
                    for (int j = i + 1; j < size; j++, Selected = j)
                    {
                        Checks++;
                        if (sortList[j] < sortList[min_idx])
                        {

                            
                            min_idx = j;
                        }
                        Thread.Sleep(50);
                    }

                    Swaps++;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        int temp = sortList[i];
                        sortList[i] = sortList[min_idx];
                        sortList[min_idx] = temp;
                    }));
                }
            });
        }

        private void insertion_Click(object sender, RoutedEventArgs e)
        {
            int size = sortList.Count;
            Checks = 0;
            Swaps = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                int n = size;
                for (int i = 1; i < n; ++i, Selected = i)
                {
                    int key = sortList[i];
                    int j = i - 1;

                    while (j >= 0 && sortList[j] > key)
                    {
                        Checks++;
                        Swaps++;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            sortList[j + 1] = sortList[j];
                            j = j - 1;
                        }));
                        Thread.Sleep(50);


                    }
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        Swaps++;
                        sortList[j + 1] = key;
                    }));
                    Thread.Sleep(50);
                }
            });

        }

        private void comb_Click(object sender, RoutedEventArgs e)
        {
            int size = sortList.Count;
            Checks = 0;
            Swaps = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                int n = size;

                int gap = n;

                bool swapped = true;

                while (gap != 1 || swapped == true)
                {

                    gap = getNextGap(gap);

                    swapped = false;

                    for (int i = 0; i < n - gap; i++, Selected = i)
                    {
                        Checks++;
                        if (sortList[i] > sortList[i + gap])
                        {
                            Swaps++;
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                int temp = sortList[i];
                                sortList[i] = sortList[i + gap];
                                sortList[i + gap] = temp;
                                swapped = true;
                            }));
                        }
                    }
                    Thread.Sleep(50);
                }
                Thread.Sleep(50);
            });
        }

        // Hilfsfunktion für Comb
        private int getNextGap(int gap)
        {
            gap = (gap * 10) / 13;
            if (gap < 1)
                return 1;
            return gap;
        }

        private void shell_Click(object sender, RoutedEventArgs e)
        {
            int size = sortList.Count;
            Checks = 0;
            Swaps = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                int n = size;

                for (int gap = n / 2; gap > 0; gap /= 2, Selected = gap)
                {
                    for (int i = gap; i < n; i++, Selected = i)
                    {
                        int temp = sortList[i];
                        int j = i;

                        while (j >= gap && sortList[j - gap] > temp)
                        {
                            Checks++;
                            Swaps++;

                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                sortList[j] = sortList[j - gap];
                                j -= gap;
                            }));
                            Thread.Sleep(50);
                        }

                        Swaps++;

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            sortList[j] = temp;
                        }));

                        Thread.Sleep(50);
                    }
                }
            });
        }

        private void heap_Click(object sender, RoutedEventArgs e)
        {

        }

        private void merge_Click(object sender, RoutedEventArgs e)
        {
            int size = sortList.Count;
            Checks = 0;
            Swaps = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                if (0 < size)
                {

                    // Find the middle point
                    int m = 0 + (size - 0) / 2;

                    // Sort first and second halves
                    mergeSort(sortList, 0, m);
                    mergeSort(sortList, m + 1, size);



                    // Merge the sorted halves
                    // Find sizes of two
                    // subarrays to be merged
                    int n1 = m - 0 + 1;
                    int n2 = size - m;

                    // Create temp arrays
                    int[] L = new int[n1];
                    int[] R = new int[n2];
                    int i, j;

                    // Copy data to temp arrays
                    for (i = 0; i < n1; ++i)
                        Swaps++;
                        

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            L[i] = sortList[0 + i];
                        }));
                    for (j = 0; j < n2; ++j)
                        Swaps++;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            R[j] = sortList[m + 1 + j];
                        }));
                    
                        

                    // Merge the temp arrays

                    // Initial indexes of first
                    // and second subarrays
                    i = 0;
                    j = 0;

                    // Initial index of merged
                    // subarray array
                    int k = 0;
                    while (i < n1 && j < n2)
                    {
                        Checks++;
                        if (L[i] <= R[j])
                        {
                            Swaps++;
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                sortList[k] = L[i];
                            }));

                            i++;
                        }
                        else
                        {
                            Swaps++;
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                sortList[k] = R[j];
                            }));
                            j++;
                        }
                        k++;
                    }

                    // Copy remaining elements
                    // of L[] if any
                    while (i < n1)
                    {
                        Checks++;
                        Swaps++;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            sortList[k] = L[i];
                        }));
                        i++;
                        k++;
                    }

                    // Copy remaining elements
                    // of R[] if any
                    while (j < n2)
                    {
                        Checks++;
                        Swaps++;

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            sortList[k] = R[j];
                        }));
                        
                        j++;
                        k++;
                    }
                }
            });
        }

        private void mergeSort(ObservableCollection<int> arr, int l, int r)
        {

            if (l < r)
            {

                // Find the middle point
                int m = l + (r - l) / 2;

                // Sort first and second halves
                mergeSort(arr, l, m);
                mergeSort(arr, m + 1, r);

                // Merge the sorted halves


                // Merge the sorted halves
                // Find sizes of two
                // subarrays to be merged
                int n1 = m - l + 1;
                int n2 = r - m;

                // Create temp arrays
                int[] L = new int[n1];
                int[] R = new int[n2];
                int i, j;

                // Copy data to temp arrays
                for (i = 0; i < n1; ++i)
                    Swaps++;


                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        L[i] = sortList[l + i];
                    }));
                for (j = 0; j < n2; ++j)
                    Swaps++;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        R[j] = sortList[m + 1 + j];
                    }));



                // Merge the temp arrays

                // Initial indexes of first
                // and second subarrays
                i = 0;
                j = 0;

                // Initial index of merged
                // subarray array
                int k = 0;
                while (i < n1 && j < n2)
                {
                    Checks++;
                    if (L[i] <= R[j])
                    {
                        Swaps++;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            sortList[k] = L[i];
                        }));

                        i++;
                    }
                    else
                    {
                        Swaps++;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            sortList[k] = R[j];
                        }));
                        j++;
                    }
                    k++;
                }

                // Copy remaining elements
                // of L[] if any
                while (i < n1)
                {
                    Checks++;
                    Swaps++;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        sortList[k] = L[i];
                    }));
                    i++;
                    k++;
                }

                // Copy remaining elements
                // of R[] if any
                while (j < n2)
                {
                    Checks++;
                    Swaps++;

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        sortList[k] = R[j];
                    }));

                    j++;
                    k++;
                }
            }
        
        }
        




        private void quick_Click(object sender, RoutedEventArgs e)
        {
            int size = sortList.Count;
            Checks = 0;
            Swaps = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                if (0 < size - 1)
                {

                    // pi is the partition return index of pivot
                    int pi = quick_partition(0, size - 1);

                    // recursion calls for smaller elements
                    // and greater or equals elements
                    quickSort(0, pi - 1);
                    quickSort(pi + 1, size - 1);
                }
            });
        }

        void quickSort(int low, int high)
        {
            if (low < high)
            {

                // pi is the partition return index of pivot
                int pivot = sortList[high];

                int i = low - 1;

                for (int j = low; j <= high - 1; j++, Selected = i)
                {
                    Checks++;
                    if (sortList[j] < pivot)
                    {
                        

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            i++;
                            Swaps++;
                            int temp2 = sortList[i];
                            sortList[i] = sortList[j];
                            sortList[j] = temp2;

                        }));
                    }
                }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    Swaps++;
                    int temp = sortList[i + 1];
                    sortList[i] = sortList[high];
                    sortList[high] = temp;
                    
                }));

                int pi = i + 1;

                // recursion calls for smaller elements
                // and greater or equals elements
                quickSort(low, pi - 1);
                quickSort(pi + 1, high);
            }
        }

        private int quick_partition(int low, int high)
        {

            int pivot = sortList[high];

            int i = low - 1;

            for (int j = low; j <= high - 1; j++, Selected = j)
            {
                Checks++;
                if (sortList[j] < pivot)
                {
                    

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        i++;
                        Swaps++;
                        int temp2 = sortList[i];
                        sortList[i] = sortList[j];
                        sortList[j] = temp2;
                    }));
                }
            }

            this.Dispatcher.Invoke(new Action(() =>
            {
                Swaps++;
                int temp = sortList[i + 1];
                sortList[i] = sortList[high];
                sortList[high] = temp;
            }));

            
            return i + 1;
        }

    }

    }

