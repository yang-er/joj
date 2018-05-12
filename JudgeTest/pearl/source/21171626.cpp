//
//  main.cpp
//  pearl
//
//  Created by 赵婉婷 on 2018/5/11.
//  Copyright © 2018年 赵婉婷. All rights reserved.
//

#include <cstdio>
#include <queue>
#include <functional>
using namespace std;
priority_queue<int, vector<int>, greater<int> > PriQ;
int main() {
    int n;
    scanf("%d", &n);
    for (int x, i = 1; i <= n; ++i) {
        scanf("%d", &x);
        PriQ.push(x);
    }
    int x, y, ans = 0;
    while (PriQ.size() > 1) {
        x = PriQ.top();
        PriQ.pop();
        y = PriQ.top();
        PriQ.pop();
        ans += x + y;
        PriQ.push(x + y);
    }
    printf("%d\n", ans);
    return 0;
}
