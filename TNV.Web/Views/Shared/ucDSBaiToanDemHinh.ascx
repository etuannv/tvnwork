<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="content-box">
    <div class="content-title grad-blue">
        <a href="/hoi-dap">Bài toán đếm hình</a>
    </div>
    <div class="feature">
        <div class="scroll">
            <script type="text/javascript">
                $(function () {
                    $("#baitoandemhinh").accordion({
                        collapsible: true,
                        active: 'none',
                        heightStyle: "content"
                    });
                });
            </script>
            <div id="baitoandemhinh">
                <h3>
                    N1. Đoạn thẳng</h3>
                <div>
                    <ol class="skill-tree-skills">
                        <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanDemHinh/CLS1847290691/DemDoanThang">
                            <span class="skill-tree-skill-number">N.11</span><span class="skill-tree-skill-name">Đếm đoạn thẳng</span></a></li>
                        
                    </ol>
                </div>

                <h3>
                    N2. Tam giác</h3>
                <div>
                    <ol class="skill-tree-skills">
                        <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanDemHinh/CLS1847290691/DemTamGiac">
                            <span class="skill-tree-skill-number">N.12</span><span class="skill-tree-skill-name">Đếm tam giác</span></a></li>
                    </ol>
                </div>

                <h3>
                    N3. Tứ giác</h3>
                <div>
                    <ol class="skill-tree-skills">
                        <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanDemHinh/CLS1847290691/DemTuGiac">
                            <span class="skill-tree-skill-number">N.13</span><span class="skill-tree-skill-name">Đếm tứ giác</span></a></li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
</div>
