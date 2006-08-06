//
// Boite de dialogue elementaire
// avec etiquette, champ de texte,
// boutons Ok et Annuler
//	      (3me version)
using System;
namespace org.jmol.jcamp
{
	[Serializable]
	public class SaisieDlg:System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button Ok;
		private System.Windows.Forms.Button Annuler;
		private System.Windows.Forms.TextBox Saisie;
		public bool OkStatus;
		public bool fin;
		//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
		public SaisieDlg(System.Windows.Forms.Form frame, System.String Titre, System.String nomChamp):base()
		{
			//UPGRADE_TODO: Constructor 'java.awt.Dialog.Dialog' was converted to 'SupportClass.DialogSupport.SetDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogDialog_javaawtFrame_javalangString_boolean'"
			SupportClass.DialogSupport.SetDialog(this, frame, Titre);
			fin = false;
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.awt.Panel' and 'System.Windows.Forms.Panel' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Windows.Forms.Panel text = new System.Windows.Forms.Panel();
			System.Windows.Forms.Label temp_Label2;
			temp_Label2 = new System.Windows.Forms.Label();
			temp_Label2.Text = nomChamp;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = temp_Label2;
			text.Controls.Add(temp_Control);
			//UPGRADE_TODO: Constructor 'java.awt.TextField.TextField' was converted to 'System.Windows.Forms.TextBox' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtTextFieldTextField_int'"
			Saisie = new System.Windows.Forms.TextBox();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			text.Controls.Add(Saisie);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			Controls.Add(text);
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.awt.Panel' and 'System.Windows.Forms.Panel' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Windows.Forms.Panel barre = new System.Windows.Forms.Panel();
			System.Windows.Forms.Button temp_Button;
			temp_Button = new System.Windows.Forms.Button();
			temp_Button.Text = "Ok";
			Ok = temp_Button;
			System.Windows.Forms.Button temp_Button2;
			temp_Button2 = new System.Windows.Forms.Button();
			temp_Button2.Text = "Cancel";
			Annuler = temp_Button2;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			barre.Controls.Add(Ok);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			barre.Controls.Add(Annuler);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			Controls.Add(barre);
			Size = new System.Drawing.Size(500, 120);
		}
		//UPGRADE_NOTE: The equivalent of method 'java.awt.Component.handleEvent' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		public bool handleEvent(Event evt)
		{
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			switch (evt.id)
			{
				
				//UPGRADE_ISSUE: Field 'java.awt.Event.WINDOW_DESTROY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
				case Event.WINDOW_DESTROY: 
					fini(false);
					break;
				
				//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
				case Event.ACTION_EVENT: 
					//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
					if (evt.target == Ok)
						fini(true);
					//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
					if (evt.target == Annuler)
						fini(false);
					break;
				}
			//UPGRADE_ISSUE: Method 'java.awt.Component.handleEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtComponenthandleEvent_javaawtEvent'"
			return base.handleEvent(evt);
		}
		//UPGRADE_NOTE: The equivalent of method 'java.awt.Component.keyDown' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		public bool keyDown(Event evt, int key)
		{
			
			if ((char) key == '\n')
			{
				fini(true);
				return true;
			}
			//UPGRADE_ISSUE: Method 'java.awt.Component.keyDown' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtComponentkeyDown_javaawtEvent_int'"
			return base.keyDown(evt, key);
		}
		private void  fini(bool status)
		{
			OkStatus = status;
			Dispose();
			fin = true;
		}
		public virtual System.String lisSaisie()
		{
			return Saisie.Text;
		}
		//UPGRADE_NOTE: Since the declaration of the following entity is not virtual in .NET the modifier new was added. References to it may have been changed to InvokeMethodAsVirtual, GetPropertyAsVirtual or SetPropertyAsVirtual. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1195'"
		new virtual public void  ShowDialog()
		{
			//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
			base.ShowDialog();
			Saisie.Focus();
		}
	}
}