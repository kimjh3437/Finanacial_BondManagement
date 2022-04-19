using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task LoadData()
        {
            objectivesFunctions = new List<Variables>();
            constraints = new List<List<Variables>>();
            rHS = new List<Variables>();
            objectivesFunctions_Original = new List<Variables>();
        }

        //______________________________________
        //
        // Content Methods 
        //______________________________________
        public async Task SimplexMethod()
        {
            // first change the constraints to negative when its maximzation problem
            if (isMaximization)
            {
                foreach (var item in objectivesFunctions)
                {
                    item.Variable *= -1;
                }
            }

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
                                        Sign = 2
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
                                        Sign = 2
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
                                        Sign = 2
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
                                        Sign = 2
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
                int index = objectivesFunctions.IndexOf(item);

                return true;
            }
            else
                return false;
            // finds the most negative point 
            
        }


        //______________________________________
        //
        // Binding  Models 
        //______________________________________
        public bool isMaximization { get; set; }
        public List<Variables> objectivesFunctions { get; set; } // objective function that is modified 
        public List<Variables> objectiveFunctions_DualSimpledx { get; set; } // this is used when the problem requires dual simplex method 
        public List<Variables> objectivesFunctions_Original { get; set; } // stores the original objective function 

        public List<List<Variables>> constraints { get; set; }
        public List<Variables> rHS { get; set; }
        

    }
}
