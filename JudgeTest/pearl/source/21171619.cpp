#define _CRT_SBCURE_NO_DEPRECATE
#include <set>
#include <cmath>
#include <queue>
#include <stack>
#include <vector>
#include <string>
#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <iostream>
#include <algorithm>
#include <functional>
#define each(i,n) (int i=1;i<=(n);++i)
using namespace std;

const int maxn = 110;
const int INF = 0x3f3f3f3f;
typedef long long ll;

int main()
{
    priority_queue<int, vector<int>, greater<int> > Q;
    int n;
    cin>>n;
    if(n==0){
        printf("0\n");
        exit(0);
        }
    while(n--){
        int temp;
        scanf("%d",&temp);
        Q.push(temp);
    }
    int ans=0;
    while(Q.size()!=1){
        int a,b;
        a=Q.top();
        Q.pop();
        //printf("pop%d\n",a);
        b=Q.top();
        Q.pop();
        //printf("pop%d\n",b);
        int c=a+b;
        ans+=c;
        Q.push(c);
        //printf("push%d\n",c);
    }
    printf("%d\n",ans);
    fclose(stdin);
    fclose(stdout);
    return 0;
}

