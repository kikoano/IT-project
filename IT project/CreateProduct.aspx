<%@ page title="" language="C#" masterpagefile="~/MasterMenu.Master" autoeventwireup="true" codebehind="createProduct.aspx.cs" inherits="IT_project.CreateProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Create Credit Product
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/createProduct.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
    <div class="row">
        <div class="col-md-3">
            <div class="form-group">
                <select runat="server" onchange="javascript:form1.submit();" onserverchange="ListSelected" id="productsList" class="form-control" size="10">
                </select>
            </div>
            <button ClientIDMode="Static" runat="server" id="addNew" onclick="if (!validate2()) {return false;}" onserverclick="AddProduct" class="btn btn-success">Додади</button>
            <button ClientIDMode="Static" runat="server" id="remove" onserverclick="Remove" class="btn btn-danger">Избриши</button>
            <div id="lblError3" class="alert alert-danger" style="display: none;">
            </div>
        </div>
        <div class="col-md-9">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-horizontal">
                        <fieldset>
                            Основни параметри
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="productName">Назив*</label>
                                <div class="col-md-7">
                                    <input ClientIDMode="Static" runat="server" id="productName" name="productName" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="IznozDo">Износ до*</label>
                                <div class="col-md-7">
                                    <input ClientIDMode="Static" runat="server" id="IznozDo" name="IznozDo" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="rokMeseciDo">Рок(месеци)*</label>
                                <div class="col-md-7">
                                    <input ClientIDMode="Static" runat="server" id="rokMeseciDo" name="rokMeseciDo" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="kamStapka">Каматна стапка*</label>
                                <div class="col-md-7">
                                    <input ClientIDMode="Static" runat="server" id="kamStapka" name="kamStapka" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="promoPeriod">Промотивен период</label>
                                <div class="col-md-7">
                                    <input runat="server" id="promoPeriod" name="promoPeriod" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="promoStavka">Промотивна стапка</label>
                                <div class="col-md-7">
                                    <input runat="server" id="promoStavka" name="promoStavka" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="vidOtplata">Вид одплата*</label>
                                <div class="col-md-7">
                                    <select ClientIDMode="Static" runat="server" id="vidOtplata" name="vidOtplata" class="form-control">
                                        <option value="0">--Избери--</option>
                                        <option value="1">Ануитетски</option>
                                        <option value="2">Намалувачки рати</option>
                                    </select>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-horizontal">
                        <fieldset>
                            Провизии при одобрување(енднократни)
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="provAplIznos">Апликација(износ)</label>
                                <div class="col-md-7">
                                    <input runat="server" id="provAplIznos" name="provAplIznos" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="provAplProcent">Апликација(%)</label>
                                <div class="col-md-7">
                                    <input runat="server" id="provAplProcent" name="provAplProcent" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="provDrugo">Провизија друго</label>
                                <div class="col-md-7">
                                    <input runat="server" id="provDrugo" name="provDrugo" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="form-horizontal">
                        <fieldset>
                            Годишни провизии
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="GProvUpravProcent">Управувачка(%)</label>
                                <div class="col-md-7">
                                    <input runat="server" id="GProvUpravProcent" name="GProvUpravProcent" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="GProvDrugo">Управувачка(износ)</label>
                                <div class="col-md-7">
                                    <input runat="server" id="GProvDrugo" name="GProvDrugo" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="form-horizontal">
                        <fieldset>
                            Месечни провизии
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="MProvUpravProcent">Управувачка(%)</label>
                                <div class="col-md-7">
                                    <input runat="server" id="MProvUpravProcent" name="MProvUpravProcent" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-5 control-label" for="MProvDrugo">Управувачка(износ)</label>
                                <div class="col-md-7">
                                    <input runat="server" id="MProvDrugo" name="MProvDrugo" type="text" placeholder="" class="form-control input-md">
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
