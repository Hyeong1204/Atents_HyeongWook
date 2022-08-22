using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01_cods
{
    public class Human : Character  // Human 클래스는 Character 클래스를 상속 받았다.
    {
        int mp = 100;
        int maxMP = 100;

        bool IsSkill = false;

        public Human() : base() // 상속받은 부모의 생성자도 같이 실행
        {
            
        }

        public override void GenerateStatus()
        {
            base.GenerateStatus();  // Character의 GenerateStatus 함수 실행
            maxMP = rand.Next(100) + 1; // 추가한 변수만 추가로 처리
            mp = maxMP;
        }

        public override void TestPrintStatus()
        {
            base.TestPrintStatus();
            Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"┃이름\t : {name}");
            Console.WriteLine($"┃체력\t : {hp} / {maxHP}");
            Console.WriteLine($"┃MP\t : {mp} / {maxMP}");
            Console.WriteLine($"┃STR\t : {STR}");
            Console.WriteLine($"┃DEX\t : {DEX}");
            Console.WriteLine($"┃INT\t : {intellegence}");
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━┛");
        }

        public override void Attack(Character target)       // 부모 클래스의 함수를 씀
        {
            base.Attack(target);        // 부모의 Attack 함수를 쓰겠다.
            int damage = STR;

            if (IsSkill == true)
            {
                damage *= 3;
                IsSkill = false;
            }

            if(rand.NextDouble() < 0.3)   // 이 조건이 참이면 30% 안쪽으로 들어왔다.
            {
                Console.WriteLine("크리티컬!");
                damage *= 2;
            }

            Console.WriteLine($"{name}이(가) {target.Name}에게 공격을 합니다.(공격력 : {damage})");
            target.TakeDamge(damage);
        }

        public void HumanSkill(Character target)
        {
            Console.WriteLine($"{name}이 휘두르기를 사용합니다.");
            IsSkill = true;
            Attack(target);
        }

    }
}
