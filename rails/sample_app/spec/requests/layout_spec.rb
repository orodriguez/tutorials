require 'spec_helper'

describe "Layout page" do
  subject { page }
  before { visit root_path }
  
  describe "About link" do
    before { click_link 'About' }
    it { should satisfy { |p| p.current_path == about_path } }
  end
  
  describe "Help link" do
    before { click_link 'Help' }
    it { should satisfy { |p| p.current_path == help_path } }
  end
  
  describe "Contact link" do
    before { click_link 'Contact' }
    it { should satisfy { |p| p.current_path == contact_path } }
  end
end
