using System.Windows.Forms;

namespace UI
{
    partial class LoadingScreen
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            button1 = new Button();
            button1.Location = new Point(500, 500);
            button1.Name = "button1";
            button1.Size = new Size(111, 90);
            button1.TabIndex = 1;
            button1.Text = "Run simulation";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;

            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1643, 947);
            Controls.Add(button1);
            Name = "Input";
            Text = "TradeSoft - Strategy definition";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
    }
}
