using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finanacial_BondManagement.Models.Interests;
using Finanacial_BondManagement.Models.Variable;
using Finanacial_BondManagement.ViewModels.BaseVM;

namespace Finanacial_BondManagement.ViewModels.CalculationsVM
{
    public class CalculationViewModel : BaseViewModel
    {
        public CalculationViewModel()
        {
        }

        //______________________________________
        //
        // Initialize Methods 
        //______________________________________
        public void LoadData()
        {
            objectivesFunctions = new ObservableCollection<Variables>();
            constraints = new ObservableCollection<ObservableCollection<Variables>>
            {
                new ObservableCollection<Variables>(), 
                new ObservableCollection<Variables>(),
                new ObservableCollection<Variables>()
            };
            rHS = new ObservableCollection<Variables>
            {
                new Variables
                {
                    Variable = 100,
                    VarNum = 1
                },
                new Variables
                {
                    Variable = -1,
                    VarNum = 2
                },
                new Variables
                {
                    Variable = -1,
                    VarNum = 3
                }
            };
            objectivesFunctions_Original = new ObservableCollection<Variables>();
            Bases = new List<Variables>(); // requires initial 
            zValue = 0.0;
            Bonds = new ObservableCollection<Bonds>();
        }

        //______________________________________
        //
        // Content Methods 
        //______________________________________

        //
        // TODO 
        // I NEED TO FILL OUT A WAY TO POPULATE COORDINATE ON THE SIMPLEX METHOD 
        //
        public async Task<Bonds> AddBondType(double interestRate, double maturityRate )
        {
            string bondID = Guid.NewGuid().ToString();

            // create the bond 
            Bonds bond = new Bonds
            {
                bondID = bondID,
                InterestRate = interestRate,
                MaturityRate = maturityRate,
                Order = objectivesFunctions_Original.Count
            };
            Bonds.Add(bond);

            int varNum = objectivesFunctions_Original.Count;
            // add to objective function 
            objectivesFunctions_Original.Add(
                new Variables
                {
                    bondID = bondID,
                    Variable = interestRate,
                    VarNum = varNum
                });

            // change the first constraint 
            constraints[0].Add(
                new Variables
                {
                    bondID = bondID,
                    Variable = 1.0,
                    VarNum = varNum,
                    Sign = 2 
                });

            //change the second constraint - add interest rates 
            constraints[1].Add(
                new Variables
                {
                    bondID = bondID,
                    Variable = interestRate,
                    VarNum = varNum,
                    Sign = 2
                });

            //change the thrd constraint - add maturity rates 
            constraints[1].Add(
                new Variables
                {
                    bondID = bondID,
                    Variable = interestRate,
                    VarNum = varNum,
                    Sign = 2
                });

            //update objective function string 
            OnPropertyChanged("objective_function_str");
            OnPropertyChanged("constraint_one_str");
            OnPropertyChanged("constraint_two_str");
            OnPropertyChanged("constraint_three_str");           
            
            return bond; 
        }

        public async Task InitiateSimplexMethod()
        {
            // first change the constraints to negative when its maximzation problem
            if (isMaximization)
            {
                foreach (var item in objectivesFunctions)
                {
                    item.Variable *= -1;
                }
            }
            await ValidationCheck();
            await AddSlackArtificialVariables();
            bool status = false;
            while (!status)
            {
                status = await IterationPivot();
            }

            // 
            // return result 
            // TBD 
            // 

        }

        // overall order 
        // Validation - addSlackArtifical - Pivot (iteration)

        //checks if the condition allows forward 
        public async Task ValidationCheck()
        {
            // get rid of all the negative rHS 
            // first check if RHS variables are negative 
            if (rHS.Where(x => x.Variable < 0).Any())
            {
                var negs = rHS.Where(x => x.Variable < 0).ToList();
                foreach (var n in negs)
                {
                    // change the sign 
                    n.Sign *= -1; 

                    // find all index of RHS and multiply by -1 on the constraints
                    if (n.Variable < 0)
                    {
                        int index = negs.IndexOf(n);
                        foreach (var element in constraints[index])
                        {
                            element.Variable = element.Variable * -1;
                        }
                    }
                }
            }

            //
            // Mode validation to be updated 
            //
        }
        public async Task AddSlackArtificialVariables()
        {

            List<int> indices = new List<int>();

            // it first should consider doing the negative RHS values 
            // but this is not considered at the moment 
            var items = rHS.Where(x => x.Variable < 0).ToList();
            if(items != null || items.Count != 0)
            {

            }
            foreach(var item in rHS)
            {
                // method does not consider equality sign "=" 

                // if the rhs sign in greater than or equal to ">"
                if(item.Sign == 1)
                {
                    int index = rHS.IndexOf(item);
                    if (!indices.Contains(index))
                    {
                        int count = 0;
                        foreach (var constraint in constraints)
                        {
                            
                            int ct = constraint.Count;
                            Bases.Add(new Variables
                            {
                                VarNum = ct
                            });
                            if (count == index)
                            {                              
                                constraint.Add(
                                    new Variables
                                    {
                                        Variable = -1,
                                        IsSlack = true,
                                        IsArtificial = false,
                                        IsRegular = false,
                                        IsBasic = false,
                                        Sign = 2,
                                        VarNum = ct
                                    });
                            }
                            else
                            {
                                constraint.Add(
                                    new Variables
                                    {
                                        Variable = 0,
                                        IsSlack = false,
                                        IsArtificial = false,
                                        IsRegular = false,
                                        IsBasic = false,
                                        Sign = 2,
                                        VarNum = ct
                                    });
                            }
                            count++;

                        }
                    }
                    
                }

                // if the rhs sign in less than or equal to "<"
                if (item.Sign == -1)
                {
                    int index = rHS.IndexOf(item);
                    if (!indices.Contains(index))
                    {
                        int count = 0;
                        foreach (var constraint in constraints)
                        {
                            if (count == index)
                            {
                                constraint.Add(
                                    new Variables
                                    {
                                        Variable = 1.0,
                                        IsSlack = true,
                                        IsArtificial = false,
                                        IsRegular = false,
                                        IsBasic = false,
                                        Sign = 2,
                                        VarNum = constraint.Count
                                    });
                            }
                            else
                            {
                                constraint.Add(
                                    new Variables
                                    {
                                        Variable = 0.0,
                                        IsSlack = false,
                                        IsArtificial = false,
                                        IsRegular = false,
                                        IsBasic = false,
                                        Sign = 2,
                                        VarNum = constraint.Count
                                    });
                            }
                            count++;

                        }
                    }
                    
                }
            }
        }
        public async Task<bool> IterationPivot()
        {
            if (objectivesFunctions.Where(x => x.Variable < 0).Any())
            {
                var item = objectivesFunctions.Where(x => x.Variable < 0).Max();
                int pivotPointColumnIndex = objectivesFunctions.IndexOf(item);

                int count = 0;
                double pivotPoint = -1;
                int pivotPointRowIndex = 0; 

                // compare and contrast - ultimately discover pivot point 
                foreach(var rhs in rHS)
                {     
                    double ratio = rhs.Variable / (constraints[count][pivotPointColumnIndex].Variable);
                    if (ratio > 0 && (ratio < pivotPoint || pivotPoint < 0))
                    {
                        pivotPointRowIndex = rHS.IndexOf(rhs);
                        pivotPoint = ratio; 
                    } 
                    count++; 
                }
                Bases[pivotPointRowIndex] = rHS[pivotPointRowIndex];
                


                foreach(var constraint in constraints)
                {
                    int currentIndex = constraints.IndexOf(constraint);
                    if (currentIndex != pivotPointRowIndex)
                    {
                        // implement pivots on constraints 
                        double pivotRatio = constraint[pivotPointColumnIndex].Variable / constraints[pivotPointRowIndex][pivotPointColumnIndex].Variable *-1;
                        foreach(var constraint_inner in constraint)
                        {
                            constraint_inner.Variable = constraint_inner.Variable + constraints[pivotPointRowIndex][currentIndex].Variable * pivotRatio;
                        }

                        // implement pivots on RHS 
                        rHS[currentIndex].Variable = rHS[currentIndex].Variable + rHS[pivotPointRowIndex].Variable * pivotRatio;
                    }
                }

                // implement pivots on obj the most bottom parts 
                double pvtRatio = objectivesFunctions[pivotPointColumnIndex].Variable / constraints[pivotPointRowIndex][pivotPointColumnIndex].Variable * -1;
                foreach (var obj in objectivesFunctions)
                {
                    int ind = objectivesFunctions.IndexOf(obj);
                    obj.Variable = obj.Variable + constraints[pivotPointRowIndex][ind].Variable * pvtRatio;
                }

                // implement pivot on obj Z value 
                zValue = zValue + rHS[pivotPointRowIndex].Variable * pvtRatio;

                // end of pivot 
                return true;
            }
            else
                return false;
            // finds the most negative point 
            
        }

        // list out the results in order from x_1 to x_n 
        public async Task ProvideResults()
        {
            // provide optimal solutions 
            int count = 1;
            while (count < constraints[0].Count)
            {
                var item = Bases.Where(x => x.VarNum == count).FirstOrDefault();
                if(item == null)
                {
                    Results.Add(new Variables
                    {
                        VarNum = count,
                        Variable = 0.0
                    });
                }
                else
                {
                    Results.Add(item);
                }
            }
        }


        //______________________________________
        //
        // Binding  Models 
        //______________________________________
        public bool isMaximization { get; set; }
        public double zValue { get; set; }
        public ObservableCollection<Variables> objectivesFunctions { get; set; } // objective function that is modified 
        public ObservableCollection<Variables> objectiveFunctions_DualSimpledx { get; set; } // this is used when the problem requires dual simplex method 
        public ObservableCollection<Variables> objectivesFunctions_Original { get; set; } // stores the original objective function 


        public ObservableCollection<Bonds> Bonds { get; set; }
        public ObservableCollection<ObservableCollection<Variables>> constraints { get; set; } // maximum three constraints 
        // first - sum should not exceed 100 
        // second - average 
        public ObservableCollection<Variables> rHS { get; set; }
        public List<Variables> Bases { get; set; } // bases // only varNum matters to "Bases" 

        private string _obj_str;
        private double _targetInterest, _targetMaturity; 
        public string objective_function_str
        {
            get
            {
                String str = "";
                foreach (var item in Bonds)
                {
                    int index = Bonds.IndexOf(item);
                    if (index + 1 == Bonds.Count)
                    {
                        str += $"{item.InterestRate}x_{(index + 1)}";
                    }
                    else
                    {
                        str += $"{item.InterestRate}x_{(index + 1)} ";
                    }
                    
                    
                }
                str = str.Replace(" ", "+");
                return str; 
            }
            set
            {
                _obj_str = value;
                OnPropertyChanged();
            }
        }
        public string constraint_one_str
        {
            get
            {
                String str = "";
                int index = 0; 
                foreach(var item in constraints[0])
                {      
                    if(index + 1 == constraints[0].Count)
                    {
                        str += $"{item.Variable}x_{item.VarNum}";
                    }
                    else
                    {
                        str += $"{item.Variable}x_{item.VarNum} ";
                    }    
                    index++;           
                }
                str = str.Replace(" ", "+");
                str += " <= 100";
                return "";
            }
        }
        public string constraint_two_str
        {
            get
            {
                String str = "";
                int index = 0;
                int toteCount = constraints[1].Count;
                foreach (var item in constraints[1])
                {
                    if(index == 0)
                    {
                        if(index+1 == constraints[1].Count)
                        {
                            str += $"({item.Variable}/{toteCount}x_{item.VarNum}";
                        }
                        else
                        {
                            str += $"({item.Variable}/{toteCount}x_{item.VarNum} ";
                        }
                    }
                    if(item.Variable < 0)
                    {
                        //when negative 
                        str += $".{item.Variable}/{toteCount}x_{item.VarNum}";
                    }
                    else
                    {
                        // when positive 
                        str += $" {item.Variable}/{toteCount}x_{item.VarNum}";
                    }
                    
                    index++;
                }
                str = str.Replace(" ", "+");
                str = str.Replace(".", "-");
                str += $" <= {targetInterest}";
                return str;
            }
        }
        public string constraint_three_str
        {
            get
            {
                String str = "";
                int index = 0;
                int toteCount = constraints[2].Count;
                foreach (var item in constraints[2])
                {
                    if (index == 0)
                    {
                        if (index + 1 == constraints[2].Count)
                        {
                            str += $"({item.Variable}/{toteCount}x_{item.VarNum}";
                        }
                        else
                        {
                            str += $"({item.Variable}/{toteCount}x_{item.VarNum} ";
                        }
                    }
                    if (item.Variable < 0)
                    {
                        //when negative 
                        str += $".{item.Variable}/{toteCount}x_{item.VarNum}";
                    }
                    else
                    {
                        // when positive 
                        str += $" {item.Variable}/{toteCount}x_{item.VarNum}";
                    }

                    index++;
                }
                str = str.Replace(" ", "+");
                str = str.Replace(".", "-");
                str += $" <= {targetMaturity}";
                return str;
            }
        }
        public double targetInterest
        {
            get
            {
                return _targetInterest;
            }
            set
            {
                _targetInterest = value;
                OnPropertyChanged();
            }
        }
        public double targetMaturity
        {
            get
            {
                return _targetMaturity;
            }
            set
            {
                _targetMaturity = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Variables> Results { get; set; }
     


        

    }
}
