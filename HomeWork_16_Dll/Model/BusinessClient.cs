using System;

namespace HomeWork_16_WPF.Model
{
    public class BusinessClient : Client
    {
        /// <summary>
        /// Хранит в тексовом виде к какому департаменту относится клиент
        /// </summary>
        public override string Status
        {
            get
            {
                return Const.businessName;
            }
        }

        
        public BusinessClient() : base($"{Const.businessName} - {Guid.NewGuid().ToString().Substring(0, 5)}")
        {
        }
        public BusinessClient(string Name, uint Money) : base(Name, Money)
        {
        }

        /// <summary>
        /// Возвращяет enum к какому департаменту относится клиент
        /// </summary>
        /// <returns></returns>
        public override BankDepartment BankDepartmentProp
        {
            get
            {
                return BankDepartment.BusinessDepartment;
            }
        }
    }
}
