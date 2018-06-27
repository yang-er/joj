#include<iostream>
#include<queue>
#include<vector>

using namespace std;

struct data
{
    int num;
    string name;
    int grade[10];
    int total;
};

struct cmp
{
    bool operator()(data* a,data* b)const
    {
         return a->total < b->total;
    }
}
   
priority_queue<data*,vector<data*>,cmp>grade;

int main()
{
    int i,j,num_people;
    data *p;
    cin >> num_people;
    for(i = 0;i < num_people;i++)
    {
        p = new data;
        cin >> p->num;
        cin >> p->name;
        for(j = 0;j < 10;j++)
        {
            cin >> p->grade[j];
            total += p->grade[j];
        }
        grade.push(p);
    }
    while(!grade.empty())
    {
        p = grade.top();
        cout << p->num << " " << p->name << p->total;
        grade.pop();
    }
    return 0;
}