<!doctype html>
<html lang="cn">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <link rel="stylesheet" href="css/bootstrap.css">
    <link rel="stylesheet" href="css/custom.css">
    <title>�ύ״̬ - JOJ Test Project</title>
</head>
<body>
    <div class="container pt-4">
        <ul class="nav nav-pills mb-4">
            <li class="nav-item">
                <a class="nav-link" href="/">��ҳ</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/submit.php?tid=">�����ύ</a>
            </li>
            <li class="nav-item">
                <a class="nav-link active" href="status.php">״̬�鿴</a>
            </li>
        </ul>
        <table class="table table-responsive-lg">
            <thead>
                <tr>
                    <th scope="col" style="width:7%;min-width:70px">#</th>
                    <th scope="col" style="width:16%;min-width:160px">�ύʱ��</th>
                    <th scope="col" style="width:25%;min-width:250px">���н��</th>
                    <th scope="col" style="width:6%;min-width:60px">���</th>
                    <th scope="col" style="width:10%;min-width:100px">ִ��ʱ��</th>
                    <th scope="col" style="width:10%;min-width:100px">ִ���ڴ�</th>
                    <th scope="col" style="width:10%;min-width:100px">���볤��</th>
                    <th scope="col" style="width:6%;min-width:60px">����</th>
                    <th scope="col" style="width:10%;min-width:100px">�ύ��</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th scope="row">7</th>
                    <td>2018/5/18 23:28:00</td>
                    <td class="state">Pending</td>
                    <td>1006</td>
                    <td>31ms</td>
                    <td>1948K</td>
                    <td><a href="status.php?id=7">900B</a></td>
                    <td>G++</td>
                    <td>55171102</td>
                </tr>
                <tr>
                    <th scope="row">6</th>
                    <td>2018/5/18 23:28:00</td>
                    <td class="state state-pe">Presentation Error</td>
                    <td>1006</td>
                    <td>31ms</td>
                    <td>1948K</td>
                    <td>900B</td>
                    <td>G++</td>
                    <td>55171102</td>
                </tr>
                <tr>
                    <th scope="row">5</th>
                    <td>2018/5/18 23:28:00</td>
                    <td class="state state-wa">Wrong Answer</td>
                    <td>1006</td>
                    <td>31ms</td>
                    <td>1948K</td>
                    <td>900B</td>
                    <td>G++</td>
                    <td>55171102</td>
                </tr>
                <tr>
                    <th scope="row">4</th>
                    <td>2018/5/18 23:28:00</td>
                    <td class="state state-ce">Compile Error</td>
                    <td>1006</td>
                    <td>31ms</td>
                    <td>1948K</td>
                    <td>900B</td>
                    <td>G++</td>
                    <td>55171102</td>
                </tr>
                <tr>
                    <th scope="row">3</th>
                    <td>2018/5/18 23:28:00</td>
                    <td class="state state-le">Time Limit Exceeded</td>
                    <td>1006</td>
                    <td>31ms</td>
                    <td>1948K</td>
                    <td>900B</td>
                    <td>G++</td>
                    <td>55171102</td>
                </tr>
                <tr>
                    <th scope="row">2</th>
                    <td>2018/5/18 23:28:00</td>
                    <td class="state state-re">Runtime Error</td>
                    <td>1006</td>
                    <td>31ms</td>
                    <td>1948K</td>
                    <td>900B</td>
                    <td>G++</td>
                    <td>55171102</td>
                </tr>
                <tr>
                    <th scope="row">1</th>
                    <td>2018/5/18 23:28:00</td>
                    <td class="state state-ac">Accepted</td>
                    <td>1006</td>
                    <td>31ms</td>
                    <td>1948K</td>
                    <td>900B</td>
                    <td>G++</td>
                    <td>55171102</td>
                </tr>
            </tbody>
        </table>
        <div class="row">
            <div class="col-md-6 order-1">
                <nav aria-label="Page navigation example">
                    <ul class="pagination">
                        <li class="page-item">
                            <a class="page-link" href="#" aria-label="Previous">
                                <span aria-hidden="true">&laquo;</span>
                                <span class="sr-only">Previous</span>
                            </a>
                        </li>
                        <li class="page-item"><a class="page-link" href="#">1</a></li>
                        <li class="page-item"><a class="page-link" href="#">2</a></li>
                        <li class="page-item"><a class="page-link" href="#">3</a></li>
                        <li class="page-item">
                            <a class="page-link" href="#" aria-label="Next">
                                <span aria-hidden="true">&raquo;</span>
                                <span class="sr-only">Next</span>
                            </a>
                        </li>
                    </ul>
                </nav>
            </div>
            <div class="col-md-6 order-0">
                <div class="input-group justify-content-end">
                    <select class="custom-select" style="max-width:100px">
                        <option selected>ɸѡ</option>
                        <option value="1">��Ŀ���</option>
                        <option value="2">ͨ�����</option>
                        <option value="3">ѧ��</option>
                    </select>
                    <input type="text" class="form-control" placeholder="ɸѡ����">
                    <div class="input-group-append">
                        <button class="btn btn-outline-secondary" type="button">��ѯ</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

<?php
