<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="content-box">
    <div class="content-title grad-pink text-center">
        <a href="/Toan-vui-hang-tuan">Phép toán ba số hạng</a>
    </div>
    <div class="feature">
        <div class="scroll">
            <script type="text/javascript">
                $(function () {
                    $("#basohang").accordion({
                        collapsible: true,
                        active: 'none',
                        heightStyle: "content"
                    });
                });
            </script>
            <div id="basohang">
                <h3>
                    B1. Phạm vi 10</h3>
                <div>
                    <ol class="skill-tree-skills">
                        <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanBaSoHang/CongTruPhamVi10/CLS1847290691">
                            <span class="skill-tree-skill-number">B.11</span><span class="skill-tree-skill-name">Cộng
                                trừ 3 số</span></a></li>
                    </ol>
                </div>
                <h3>
                    B2. Phạm vi 20</h3>
                <div>
                    <ol class="skill-tree-skills">
                        <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanBaSoHang/CongTruPhamVi20/CLS1847290691">
                            <span class="skill-tree-skill-number">B.21</span><span class="skill-tree-skill-name">Cộng
                                trừ 3 số hạng</span></a></li>
                    </ol>
                </div>
                <h3>
                    B3. Phạm vi 30</h3>
                <div>
                    <ol class="skill-tree-skills">
                        <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanBaSoHang/CongTruPhamVi30/CLS1847290691">
                            <span class="skill-tree-skill-number">B.31</span><span class="skill-tree-skill-name">Cộng
                                trừ 3 số hạng</span></a></li>
                    </ol>
                </div>
                <h3>
                    B4. Phạm vi 30 đến 100</h3>
                <div>
                    <ol class="skill-tree-skills">
                        <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanBaSoHang/CongTruPhamVi30Den100/CLS1847290691">
                            <span class="skill-tree-skill-number">B.41</span><span class="skill-tree-skill-name">Cộng
                                trừ 3 số hạng</span></a></li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    <a></a>
</div>
