using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// 1. Orc 클래스 완성하기
//    1.1. 오크만의 변수 추가하기(ex) 분노, 버프 등등)
//    1.2. 오크만의 함수 추가하기
// 2. human 클래스 수정하기
//      2.1. 스킬 함수 추가하기
// 3. 입력에 따라 행동하는 player 만들기
//        3.1 "1)공격   2)스킬    3)방어" 3개중 하나를 입력받아 그대로 행동하게 만들기
// 4. player와 enemy가 싸우게 만들기
namespace _01_cods
{
    public class Orc : Character
    {
        private string skill = "분노";

        /// <summary>
        /// 기본 생성자
        /// </summary>
        public Orc() : base()
        {

        }

        /// <summary>
        /// 이름을 입력받는 생성자(생성자는 상속이 안되기 때문에 항상 새로 만들어 줘야 한다.)
        /// </summary>
        /// <param name="newName">새로운 이름</param>
        public Orc(string newName) : base(newName)          // 매개변수를 받아 Character의 생성자를 실행
        {
            
        }

        public override void Attack(Character target)
        {
            base.Attack(target);
            int damage = STR;
            if (target.Barrier)
            {
                Console.WriteLine($"{target.Name}이 방어를 합니다.");
                damage *= 0;
                target.Barrier = false;
            }
            if(rand.NextDouble() < 0.3)
            {
               damage = OrcSkill(damage);
            }
            if(rand.NextDouble() < 0.3)   // 이 조건이 참이면 30% 안쪽으로 들어왔다.
            {
                Console.WriteLine("크리티컬!");
                damage *= 2;
            }
            Console.WriteLine($"{name}이(가) {target.Name}에게 공격을 합니다.(공격력 : {damage})");
            target.TakeDamge(damage);
        }

        public int OrcSkill(int damage)
        {
            Console.WriteLine($"{name}이(가) {skill}를 사용하여 타격합니다.");
            return damage * 3;
        }

        /// <summary>
        /// 스테이터스 생성
        /// </summary>
        public override void GenerateStatus()
        {
            base.GenerateStatus();
        }

        public override void PrintStatus()
        {
            base.PrintStatus();
            Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"┃이름\t : {name}");
            Console.WriteLine($"┃체력\t : {hp} / {maxHP}");
            Console.WriteLine($"┃STR\t : {STR}");
            Console.WriteLine($"┃DEX\t : {DEX}");
            Console.WriteLine($"┃INT\t : {intellegence}");
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━┛");
        }

    }



}
