using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SkillHub.Forms.Freelancer
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class FrmFreelancerOrders : Form
    {
        // ============================================================
        // REPOSITORY
        // ============================================================

        private readonly FreelancerOrderRepository _repository;

        // ============================================================
        // DATA
        // ============================================================

        private List<Order> _orders = new List<Order>();
        private Order _selectedOrder;

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FrmFreelancerOrders()
        {
            InitializeComponent();

            _repository = new FreelancerOrderRepository();

            ConfigureGrid();
            LoadOrders();
        }

        // ============================================================
        // GRID
        // ============================================================

        private void ConfigureGrid()
        {
            dgvOrders.Columns.Clear();

            DataGridViewTextBoxColumn orderColumn =
                new DataGridViewTextBoxColumn();

            orderColumn.Name = "OrderId";
            orderColumn.HeaderText = "ORDER";
            orderColumn.DataPropertyName = "OrderId";
            orderColumn.Width = 75;

            DataGridViewTextBoxColumn clientColumn =
                new DataGridViewTextBoxColumn();

            clientColumn.Name = "Client";
            clientColumn.HeaderText = "CLIENT";
            clientColumn.DataPropertyName = "ClientName";
            clientColumn.Width = 150;

            DataGridViewTextBoxColumn serviceColumn =
                new DataGridViewTextBoxColumn();

            serviceColumn.Name = "Service";
            serviceColumn.HeaderText = "SERVICE";
            serviceColumn.DataPropertyName = "ServiceTitle";
            serviceColumn.Width = 235;

            DataGridViewTextBoxColumn amountColumn =
                new DataGridViewTextBoxColumn();

            amountColumn.Name = "Amount";
            amountColumn.HeaderText = "AMOUNT";
            amountColumn.DataPropertyName = "GrossAmount";
            amountColumn.Width = 110;

            DataGridViewTextBoxColumn statusColumn =
                new DataGridViewTextBoxColumn();

            statusColumn.Name = "Status";
            statusColumn.HeaderText = "STATUS";
            statusColumn.DataPropertyName = "OrderStatus";
            statusColumn.Width = 125;

            DataGridViewTextBoxColumn dateColumn =
                new DataGridViewTextBoxColumn();

            dateColumn.Name = "Date";
            dateColumn.HeaderText = "DATE";
            dateColumn.DataPropertyName = "CreatedAt";
            dateColumn.Width = 115;

            dgvOrders.Columns.Add(orderColumn);
            dgvOrders.Columns.Add(clientColumn);
            dgvOrders.Columns.Add(serviceColumn);
            dgvOrders.Columns.Add(amountColumn);
            dgvOrders.Columns.Add(statusColumn);
            dgvOrders.Columns.Add(dateColumn);
        }

        // ============================================================
        // LOAD ORDERS
        // ============================================================

        private void LoadOrders()
        {
            try
            {
                int freelancerId = UserSession.UserId;

                _orders = _repository
                    .GetByFreelancer(freelancerId)
                    ?? new List<Order>();

                UpdateStatistics();

                ApplyFilters();

                ClearDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load your orders.\n\n" +
                    ex.Message,
                    "Orders",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // STATISTICS
        // ============================================================

        private void UpdateStatistics()
        {
            lblTotalValue.Text = _orders.Count.ToString();

            lblPlacedValue.Text =
                _orders.Count(o =>
                    string.Equals(
                        o.OrderStatus,
                        "Placed",
                        StringComparison.OrdinalIgnoreCase))
                .ToString();

            lblProgressValue.Text =
                _orders.Count(o =>
                    string.Equals(
                        o.OrderStatus,
                        "In Progress",
                        StringComparison.OrdinalIgnoreCase))
                .ToString();

            lblDeliveredValue.Text =
                _orders.Count(o =>
                    string.Equals(
                        o.OrderStatus,
                        "Delivered",
                        StringComparison.OrdinalIgnoreCase))
                .ToString();
        }

        // ============================================================
        // FILTER
        // ============================================================

        private void ApplyFilters()
        {
            string search =
                txtSearch.Text == "Search orders..."
                    ? string.Empty
                    : txtSearch.Text.Trim();

            string selectedStatus =
                cmbStatus.SelectedItem?.ToString()
                ?? "All Statuses";

            IEnumerable<Order> filtered =
                _orders;

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(o =>
                    o.OrderId.ToString()
                        .Contains(search) ||

                    (o.ClientName ?? string.Empty)
                        .IndexOf(
                            search,
                            StringComparison.OrdinalIgnoreCase) >= 0 ||

                    (o.ServiceTitle ?? string.Empty)
                        .IndexOf(
                            search,
                            StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (selectedStatus != "All Statuses")
            {
                filtered = filtered.Where(o =>
                    string.Equals(
                        o.OrderStatus,
                        selectedStatus,
                        StringComparison.OrdinalIgnoreCase));
            }

            dgvOrders.DataSource =
                filtered
                    .Select(o => new
                    {
                        o.OrderId,
                        o.ClientName,
                        o.ServiceTitle,
                        GrossAmount =
                            $"৳ {o.GrossAmount:N2}",
                        o.OrderStatus,
                        CreatedAt =
                            o.CreatedAt.ToString("dd MMM yyyy")
                    })
                    .ToList();

            if (dgvOrders.Rows.Count == 0)
            {
                ClearDetails();
            }
        }

        // ============================================================
        // SEARCH
        // ============================================================

        private void txtSearch_GotFocus(
            object sender,
            EventArgs e)
        {
            if (txtSearch.Text == "Search orders...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor =
                    Color.FromArgb(50, 60, 75);
            }
        }

        private void txtSearch_LostFocus(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search orders...";
                txtSearch.ForeColor =
                    Color.FromArgb(140, 150, 165);
            }
        }

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            if (!IsHandleCreated)
                return;

            ApplyFilters();
        }

        private void cmbStatus_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ApplyFilters();
        }

        // ============================================================
        // GRID SELECTION
        // ============================================================

        private void dgvOrders_SelectionChanged(
            object sender,
            EventArgs e)
        {
            if (dgvOrders.CurrentRow == null)
                return;

            if (dgvOrders.CurrentRow.Cells["OrderId"].Value == null)
                return;

            int orderId;

            if (!int.TryParse(
                    dgvOrders.CurrentRow.Cells["OrderId"]
                        .Value.ToString(),
                    out orderId))
            {
                return;
            }

            Order order = _orders.FirstOrDefault(
                o => o.OrderId == orderId);

            if (order == null)
                return;

            ShowOrderDetails(order);
        }

        // ============================================================
        // SHOW DETAILS
        // ============================================================

        private void ShowOrderDetails(Order order)
        {
            _selectedOrder = order;

            emptyDetailsPanel.Visible = false;

            lblOrderNumber.Text =
                $"#ORD-{order.OrderId:D4}";

            lblClientValue.Text =
                string.IsNullOrWhiteSpace(order.ClientName)
                    ? "Unknown client"
                    : order.ClientName;

            lblServiceValue.Text =
                string.IsNullOrWhiteSpace(order.ServiceTitle)
                    ? "Unknown service"
                    : order.ServiceTitle;

            lblQuantityValue.Text =
                order.Quantity.ToString();

            lblUnitPriceValue.Text =
                $"৳ {order.UnitPrice:N2}";

            lblDiscountValue.Text =
                $"৳ {order.DiscountAmount:N2}";

            lblGrossValue.Text =
                $"৳ {order.GrossAmount:N2}";

            lblEarningValue.Text =
                $"৳ {order.FreelancerEarning:N2}";

            lblCreatedValue.Text =
                order.CreatedAt.ToString("dd MMM yyyy");

            lblAcceptedValue.Text =
                FormatDate(order.AcceptedAt);

            lblDeliveredValue.Text =
                FormatDate(order.DeliveredAt);

            lblCompletedValue.Text =
                FormatDate(order.CompletedAt);

            lblDeliveryNoteValue.Text =
                string.IsNullOrWhiteSpace(order.DeliveryNote)
                    ? "No delivery note"
                    : order.DeliveryNote;

            SetStatusBadge(order.OrderStatus);

            bool canAccept =
                string.Equals(
                    order.OrderStatus,
                    "Placed",
                    StringComparison.OrdinalIgnoreCase);

            bool canDeliver =
                string.Equals(
                    order.OrderStatus,
                    "In Progress",
                    StringComparison.OrdinalIgnoreCase);

            btnAccept.Enabled = canAccept;
            btnDeliver.Enabled = canDeliver;

            btnAccept.BackColor =
                canAccept
                    ? Color.FromArgb(31, 91, 255)
                    : Color.FromArgb(220, 225, 232);

            btnAccept.ForeColor =
                canAccept
                    ? Color.White
                    : Color.FromArgb(135, 145, 158);

            btnDeliver.BackColor =
                canDeliver
                    ? Color.White
                    : Color.FromArgb(245, 247, 250);

            btnDeliver.ForeColor =
                canDeliver
                    ? Color.FromArgb(31, 91, 255)
                    : Color.FromArgb(145, 155, 168);
        }

        // ============================================================
        // STATUS BADGE
        // ============================================================

        private void SetStatusBadge(string status)
        {
            string normalized =
                status ?? string.Empty;

            lblStatusBadge.Text =
                normalized.ToUpper();

            if (normalized.Equals(
                    "Placed",
                    StringComparison.OrdinalIgnoreCase))
            {
                lblStatusBadge.BackColor =
                    Color.FromArgb(255, 244, 214);

                lblStatusBadge.ForeColor =
                    Color.FromArgb(157, 104, 0);
            }
            else if (normalized.Equals(
                    "In Progress",
                    StringComparison.OrdinalIgnoreCase))
            {
                lblStatusBadge.BackColor =
                    Color.FromArgb(228, 237, 255);

                lblStatusBadge.ForeColor =
                    Color.FromArgb(31, 91, 255);
            }
            else if (normalized.Equals(
                    "Delivered",
                    StringComparison.OrdinalIgnoreCase))
            {
                lblStatusBadge.BackColor =
                    Color.FromArgb(226, 247, 237);

                lblStatusBadge.ForeColor =
                    Color.FromArgb(22, 125, 72);
            }
            else if (normalized.Equals(
                    "Completed",
                    StringComparison.OrdinalIgnoreCase))
            {
                lblStatusBadge.BackColor =
                    Color.FromArgb(225, 244, 237);

                lblStatusBadge.ForeColor =
                    Color.FromArgb(18, 116, 72);
            }
            else
            {
                lblStatusBadge.BackColor =
                    Color.FromArgb(239, 242, 246);

                lblStatusBadge.ForeColor =
                    Color.FromArgb(105, 115, 130);
            }
        }

        // ============================================================
        // GRID STATUS STYLING
        // ============================================================

        private void dgvOrders_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvOrders.Columns[e.ColumnIndex].Name == "Status")
            {
                string status =
                    e.Value?.ToString() ?? "";

                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        8F,
                        FontStyle.Bold);

                if (status.Equals(
                        "Placed",
                        StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor =
                        Color.FromArgb(157, 104, 0);
                }
                else if (status.Equals(
                        "In Progress",
                        StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor =
                        Color.FromArgb(31, 91, 255);
                }
                else if (status.Equals(
                        "Delivered",
                        StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor =
                        Color.FromArgb(22, 125, 72);
                }
            }

            if (dgvOrders.Columns[e.ColumnIndex].Name == "Amount")
            {
                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        9F,
                        FontStyle.Bold);

                e.CellStyle.ForeColor =
                    Color.FromArgb(28, 44, 65);
            }
        }

        // ============================================================
        // ACCEPT ORDER
        // ============================================================

        private void btnAccept_Click(
            object sender,
            EventArgs e)
        {
            if (_selectedOrder == null)
                return;

            DialogResult result =
                MessageBox.Show(
                    $"Accept order #{_selectedOrder.OrderId:D4}?\n\n" +
                    "The order will move to In Progress.",
                    "Accept Order",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool success =
                    _repository.AcceptOrder(
                        _selectedOrder.OrderId,
                        UserSession.UserId);

                if (success)
                {
                    MessageBox.Show(
                        "Order accepted successfully.",
                        "Order Accepted",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadOrders();
                }
                else
                {
                    MessageBox.Show(
                        "The order could not be accepted. " +
                        "It may have already been updated.",
                        "Unable to Accept",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    LoadOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred while accepting the order.\n\n" +
                    ex.Message,
                    "Order Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DELIVER ORDER
        // ============================================================

        private void btnDeliver_Click(
            object sender,
            EventArgs e)
        {
            if (_selectedOrder == null)
                return;

            using (Form dialog = CreateDeliveryDialog())
            {
                if (dialog.ShowDialog(this) !=
                    DialogResult.OK)
                {
                    return;
                }

                TextBox noteBox =
                    dialog.Controls
                        .Find("txtDeliveryNote", true)
                        .FirstOrDefault() as TextBox;

                string note =
                    noteBox?.Text?.Trim() ?? "";

                try
                {
                    bool success =
                        _repository.DeliverOrder(
                            _selectedOrder.OrderId,
                            UserSession.UserId,
                            note);

                    if (success)
                    {
                        MessageBox.Show(
                            "Order delivered successfully.",
                            "Order Delivered",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LoadOrders();
                    }
                    else
                    {
                        MessageBox.Show(
                            "The order could not be delivered. " +
                            "It may no longer be In Progress.",
                            "Unable to Deliver",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        LoadOrders();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "An error occurred while delivering the order.\n\n" +
                        ex.Message,
                        "Order Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // ============================================================
        // DELIVERY DIALOG
        // ============================================================

        private Form CreateDeliveryDialog()
        {
            Form dialog = new Form();

            dialog.Name = "DeliveryDialog";
            dialog.Text = "Deliver Order";
            dialog.Size = new Size(520, 350);
            dialog.StartPosition =
                FormStartPosition.CenterParent;

            dialog.FormBorderStyle =
                FormBorderStyle.FixedDialog;

            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor =
                Color.FromArgb(248, 250, 253);

            Label title = new Label();

            title.AutoSize = true;
            title.Font =
                new Font(
                    "Segoe UI",
                    15F,
                    FontStyle.Bold);

            title.ForeColor =
                Color.FromArgb(27, 38, 55);

            title.Location =
                new Point(28, 25);

            title.Text =
                $"Deliver Order #{_selectedOrder.OrderId:D4}";

            Label subtitle = new Label();

            subtitle.AutoSize = true;
            subtitle.Font =
                new Font("Segoe UI", 9F);

            subtitle.ForeColor =
                Color.FromArgb(112, 124, 141);

            subtitle.Location =
                new Point(30, 58);

            subtitle.Text =
                "Add a short note for the client.";

            Label noteLabel = new Label();

            noteLabel.AutoSize = true;
            noteLabel.Font =
                new Font(
                    "Segoe UI",
                    8.5F,
                    FontStyle.Bold);

            noteLabel.ForeColor =
                Color.FromArgb(80, 92, 108);

            noteLabel.Location =
                new Point(30, 95);

            noteLabel.Text =
                "DELIVERY NOTE";

            TextBox noteBox = new TextBox();

            noteBox.Name =
                "txtDeliveryNote";

            noteBox.Multiline = true;
            noteBox.ScrollBars =
                ScrollBars.Vertical;

            noteBox.Font =
                new Font("Segoe UI", 9.5F);

            noteBox.Location =
                new Point(30, 120);

            noteBox.Size =
                new Size(440, 105);

            noteBox.BorderStyle =
                BorderStyle.FixedSingle;

            Button cancelButton =
                new Button();

            cancelButton.Text = "Cancel";
            cancelButton.DialogResult =
                DialogResult.Cancel;

            cancelButton.FlatStyle =
                FlatStyle.Flat;

            cancelButton.FlatAppearance.BorderSize = 1;
            cancelButton.FlatAppearance.BorderColor =
                Color.FromArgb(210, 216, 224);

            cancelButton.BackColor =
                Color.White;

            cancelButton.ForeColor =
                Color.FromArgb(75, 87, 103);

            cancelButton.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            cancelButton.Location =
                new Point(245, 245);

            cancelButton.Size =
                new Size(105, 38);

            Button deliverButton =
                new Button();

            deliverButton.Text =
                "Deliver Order";

            deliverButton.DialogResult =
                DialogResult.OK;

            deliverButton.FlatStyle =
                FlatStyle.Flat;

            deliverButton.FlatAppearance.BorderSize = 0;

            deliverButton.BackColor =
                Color.FromArgb(31, 91, 255);

            deliverButton.ForeColor =
                Color.White;

            deliverButton.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            deliverButton.Location =
                new Point(365, 245);

            deliverButton.Size =
                new Size(105, 38);

            dialog.AcceptButton =
                deliverButton;

            dialog.CancelButton =
                cancelButton;

            dialog.Controls.Add(title);
            dialog.Controls.Add(subtitle);
            dialog.Controls.Add(noteLabel);
            dialog.Controls.Add(noteBox);
            dialog.Controls.Add(cancelButton);
            dialog.Controls.Add(deliverButton);

            return dialog;
        }

        // ============================================================
        // REFRESH
        // ============================================================

        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            LoadOrders();
        }

        // ============================================================
        // CLEAR DETAILS
        // ============================================================

        private void ClearDetails()
        {
            _selectedOrder = null;

            emptyDetailsPanel.Visible = true;

            lblOrderNumber.Text = "#ORD-0000";
            lblStatusBadge.Text = "NO ORDER";

            lblStatusBadge.BackColor =
                Color.FromArgb(239, 242, 246);

            lblStatusBadge.ForeColor =
                Color.FromArgb(105, 115, 130);

            lblClientValue.Text = "—";
            lblServiceValue.Text = "—";
            lblQuantityValue.Text = "—";
            lblUnitPriceValue.Text = "—";
            lblDiscountValue.Text = "—";
            lblGrossValue.Text = "—";
            lblEarningValue.Text = "—";
            lblCreatedValue.Text = "—";
            lblAcceptedValue.Text = "—";
            lblDeliveredValue.Text = "—";
            lblCompletedValue.Text = "—";
            lblDeliveryNoteValue.Text = "—";

            btnAccept.Enabled = false;
            btnDeliver.Enabled = false;

            btnAccept.BackColor =
                Color.FromArgb(220, 225, 232);

            btnAccept.ForeColor =
                Color.FromArgb(135, 145, 158);

            btnDeliver.BackColor =
                Color.FromArgb(245, 247, 250);

            btnDeliver.ForeColor =
                Color.FromArgb(145, 155, 168);
        }

        // ============================================================
        // DATE FORMAT
        // ============================================================

        private string FormatDate(DateTime? date)
        {
            return date.HasValue
                ? date.Value.ToString("dd MMM yyyy")
                : "—";
        }
    }
}