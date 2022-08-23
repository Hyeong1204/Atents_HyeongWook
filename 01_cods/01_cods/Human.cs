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
        int mp = 100;           // Human은 추가로 mp를 가지고 있음.
        int maxMP = 100;
        const int DefenseCount = 3;     // 방어태세용 변수(한번 방어을 선택할 때 몇번까지 데미지가 감소하는지)
        int remainsDefenseCount = 0;    // 남아 있는 방어 횟수

        bool IsSkill = false;

        /// <summary>
        /// 기본 생성자
        /// </summary>
        public Human() : base() 
        {
            // 이게 없으면 Human(string newname) : base(newname)만 존재하게 되기 때문에
            // 상속받은 부모의 생성자도 같이 실행
        }

        /// <summary>
        /// 이름을 받는 생성자
        /// </summary>
        /// <param name="newname"></param>
        public Human(string newname) : base(newname)    // base(newname) == Character(string newName) 실행
        {

        }

        /// <summary>
        /// 스테이터스 생성용(Mp도 생성)
        /// </summary>
        public override void GenerateStatus()
        {
            base.GenerateStatus();  // Character의 GenerateStatus 함수 실행
            maxMP = rand.Next(100) + 1; // 추가한 변수만 추가로 처리
            mp = maxMP;
        }

        /// <summary>
        /// 스테이터스 창 출력
        /// </summary>
        public override void PrintStatus()
        {
            base.PrintStatus();
            Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"┃이름\t : {name}");
            Console.WriteLine($"┃체력\t : {hp} / {maxHP}");
            Console.WriteLine($"┃MP\t : {mp} / {maxMP}");
            Console.WriteLine($"┃STR\t : {STR}");
            Console.WriteLine($"┃DEX\t : {DEX}");
            Console.WriteLine($"┃INT\t : {intellegence}");
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━┛");
        }

        /// <summary>
        /// 공격 함수
        /// </summary>
        /// <param name="target">공격 대상</param>
        public override void Attack(Character target)       // 부모 클래스의 어택 함수를 씀
        {
            base.Attack(target);        // 부모의 Attack 함수를 쓰겠다.
            int damage = STR;       // 힘을 기바능로 데미지 계산

            if (IsSkill == true)        // IsSkill 이 참이면 실행
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
            target.TakeDamge(damage);   // 최종 데미지를 대상에게 전달
        }

        public void HumanSkill(Character target)        // 휴면 스킬 함수
        {
            Console.WriteLine($"{name}이 휘두르기를 사용합니다.");
            IsSkill = true;
            Attack(target);
        }

        /// <summary>
        /// 방어 함수
        /// </summary>
        public void Defaense()          // 방어 함수
        {
            Console.WriteLine("3턴간 방어를 합니다.");
            remainsDefenseCount += DefenseCount;    // 상수인 DefenseCount에 값을 remainsDefenseCount에 대입한다.
        }

        /// <summary>
        /// 받은 피해 처리 함수
        /// </summary>
        /// <param name="damage">받은 데미지</param>
        public override void TakeDamge(int damage)      // 부모클래스에 TakeDamge를 오버라이브 
        {
            // 방어 횟수가 남아 있으면
            if(remainsDefenseCount > 0)
            {
                Console.WriteLine("방어 발동! 받는 데미지가 절반 감소합니다. ");
                remainsDefenseCount--;      // 실행 할때마다 -1 시킨다.
                damage = damage >> 1;       // damage 값을 반절로 만든다.
            }
            base.TakeDamge(damage);         //부모클래스에 TakeDamge를 실행
        }

    }
}
